using UnityEngine;
using UnityEngine.XR;

namespace OneWorld
{
    /// <summary>
    /// Minimal OpenXR device-pose and controller-input rig using Unity's built-in XR API.
    /// Starts in teleport-first mode; optional stick movement and snap turning are exposed in the Inspector.
    /// </summary>
    public sealed class QuestPlayerRig : MonoBehaviour
    {
        [Header("Rig references")]
        [SerializeField] private Transform head;
        [SerializeField] private Transform leftController;
        [SerializeField] private Transform rightController;
        [SerializeField] private LineRenderer teleportRay;
        [SerializeField] private LayerMask teleportLayers = ~0;

        [Header("Comfort")]
        [SerializeField] private bool allowSmoothMove;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float snapTurnDegrees = 30f;
        [SerializeField] private float teleportRange = 10f;

        private InputDevice headDevice, leftDevice, rightDevice;
        private bool previousTrigger;
        private float nextSnapTurn;

        private void Start()
        {
            head ??= transform.Find("Head");
            leftController ??= transform.Find("Left Controller");
            rightController ??= transform.Find("Right Controller");
            RefreshDevices();
        }

        private void Update()
        {
            if (!headDevice.isValid || !leftDevice.isValid || !rightDevice.isValid) RefreshDevices();
            ApplyPose(headDevice, head);
            ApplyPose(leftDevice, leftController);
            ApplyPose(rightDevice, rightController);

            if (allowSmoothMove && leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 move))
            {
                var forward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
                var right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
                transform.position += (forward * move.y + right * move.x) * (moveSpeed * Time.deltaTime);
            }

            if (rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 turn) &&
                Mathf.Abs(turn.x) > 0.75f && Time.unscaledTime >= nextSnapTurn)
            {
                transform.RotateAround(head.position, Vector3.up, Mathf.Sign(turn.x) * snapTurnDegrees);
                nextSnapTurn = Time.unscaledTime + 0.35f;
            }

            UpdateInteraction();
        }

        private void RefreshDevices()
        {
            headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        private static void ApplyPose(InputDevice device, Transform target)
        {
            if (target == null || !device.isValid) return;
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position)) target.localPosition = position;
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation)) target.localRotation = rotation;
        }

        private void UpdateInteraction()
        {
            if (rightController == null) return;
            var ray = new Ray(rightController.position, rightController.forward);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, teleportRange, teleportLayers, QueryTriggerInteraction.Ignore);
            if (teleportRay != null)
            {
                teleportRay.enabled = true;
                teleportRay.SetPosition(0, rightController.position);
                teleportRay.SetPosition(1, hitSomething ? hit.point : ray.GetPoint(teleportRange));
                teleportRay.startColor = hitSomething && hit.collider.GetComponentInParent<WorldPortal>() != null ? Color.cyan : Color.white;
                teleportRay.endColor = teleportRay.startColor;
            }

            bool triggerDown = rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed) && pressed;
            if (triggerDown && !previousTrigger && hitSomething)
            {
                var portal = hit.collider.GetComponentInParent<WorldPortal>();
                if (portal != null)
                    portal.Enter();
                else if (hit.collider.GetComponentInParent<SceneEntryButton>() is SceneEntryButton button)
                    button.Activate();
                else if (hit.collider.GetComponentInParent<TeleportSurface>() != null)
                    transform.position += hit.point - new Vector3(head.position.x, transform.position.y, head.position.z);
            }
            previousTrigger = triggerDown;
        }
    }
}

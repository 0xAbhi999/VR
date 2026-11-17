using UnityEngine;

namespace OneWorld
{
    /// <summary>A reusable, ray-interactable entrance to a destination scene.</summary>
    public sealed class WorldPortal : MonoBehaviour
    {
        [SerializeField] private string worldName;
        [SerializeField] private string introduction;
        [SerializeField] private string destinationScene;
        [SerializeField] private Color portalColor = Color.cyan;

        public string WorldName => worldName;
        public string Introduction => introduction;
        public string DestinationScene => destinationScene;
        public Color PortalColor => portalColor;

        public void Configure(string title, string description, string sceneName, Color color)
        {
            worldName = title;
            introduction = description;
            destinationScene = sceneName;
            portalColor = color;
        }

        public void Enter()
        {
            if (string.IsNullOrWhiteSpace(destinationScene))
            {
                Debug.LogWarning($"Portal '{worldName}' has no destination scene.", this);
                return;
            }

            SceneTransition.Load(destinationScene);
        }
    }
}

using UnityEngine;

namespace OneWorld
{
    /// <summary>A simple controller-ray target that loads its configured scene.</summary>
    public sealed class SceneEntryButton : MonoBehaviour
    {
        [SerializeField] private string destination;
        public void Configure(string sceneName) => destination = sceneName;
        public void Activate() => SceneTransition.Load(destination);
    }

    /// <summary>Marks a collider as a safe destination for controller-ray teleport.</summary>
    public sealed class TeleportSurface : MonoBehaviour { }
}

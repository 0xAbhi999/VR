using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneWorld
{
    /// <summary>Small shared loading overlay for scene changes.</summary>
    public sealed class SceneTransition : MonoBehaviour
    {
        private static SceneTransition instance;
        private bool isLoading;
        private GUIStyle overlayStyle;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void Load(string sceneName)
        {
            if (instance == null)
            {
                var host = new GameObject("Scene Transition");
                instance = host.AddComponent<SceneTransition>();
            }

            if (!instance.isLoading)
                instance.StartCoroutine(instance.LoadRoutine(sceneName));
        }

        private IEnumerator LoadRoutine(string sceneName)
        {
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"Scene '{sceneName}' is not in Build Settings. Run ONE WORLD > Generate Foundation Scenes.");
                yield break;
            }

            isLoading = true;
            yield return new WaitForSecondsRealtime(0.2f);
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (operation != null && !operation.isDone)
                yield return null;
            yield return new WaitForSecondsRealtime(0.2f);
            isLoading = false;
        }

        private void OnGUI()
        {
            if (!isLoading) return;
            overlayStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 36,
                normal = { textColor = Color.white }
            };
            GUI.color = new Color(0.02f, 0.04f, 0.09f, 0.96f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "ONE WORLD\nLoading…", overlayStyle);
        }
    }
}

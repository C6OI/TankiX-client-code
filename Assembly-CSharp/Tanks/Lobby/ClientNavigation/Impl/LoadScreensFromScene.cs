using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class LoadScreensFromScene : MonoBehaviour {
        public string sceneName = string.Empty;

        public string pathToScreens = string.Empty;

        bool loaded;

        void OnEnable() {
            if (!loaded) {
                loaded = true;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                StartCoroutine(MoveScreensUnderSelf());
            }
        }

        IEnumerator MoveScreensUnderSelf() {
            yield return new WaitForSeconds(0f);

            string firstPathPart = !pathToScreens.Contains("/") ? pathToScreens : pathToScreens.Split('/')[0];
            GameObject root = SceneManager.GetSceneByName(sceneName).GetRootGameObjects().FirstOrDefault(o => o.name.Equals(firstPathPart));
            Transform screen = null;

            if (root != null) {
                string text = !pathToScreens.Contains("/") ? null : pathToScreens.Substring(pathToScreens.IndexOf('/') + 1);
                screen = text == null ? root.transform : root.transform.Find(text);
            }

            if (screen == null) {
                Debug.LogWarning("LoadScreensFromScene can't find screen at " + pathToScreens);
                yield break;
            }

            screen.SetParent(transform, false);
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
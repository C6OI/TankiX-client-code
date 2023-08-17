using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platform.Library.ClientUnityIntegration.API {
    public class SceneLoaderActivator : UnityAwareActivator<ManuallyCompleting> {
        public List<string> environmentSceneNames;

        protected override void Activate() => StartCoroutine(LoadScenes());

        IEnumerator LoadScenes() {
            for (int i = 0; i < environmentSceneNames.Count; i++) {
                SceneManager.LoadScene(environmentSceneNames[i], LoadSceneMode.Additive);
            }

            yield return new WaitForEndOfFrame();

            Complete();
        }
    }
}
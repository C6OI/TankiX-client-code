using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby.ClientNavigation.Impl {
    public static class SceneSwitcher {
        [Inject] public static NetworkService NetworkService { get; set; }

        public static void CleanAndRestart() {
            CleanPreviousScene();
            SceneManager.LoadScene(0);
        }

        public static void CleanAndSwitch(string sceneName) {
            CleanPreviousScene();
            SceneManager.LoadScene(sceneName);
        }

        public static void Clean() {
            AssetBundlesStorage.Clean();
            ServiceRegistry.Reset();
            InjectionUtils.ClearInjectionPoints(typeof(InjectAttribute));
            BaseElementCanvasScaler.MarkNeedInitialize();
            FatalErrorHandler.IsErrorScreenWasShown = false;
        }

        static void CleanPreviousScene() {
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                engine.ScheduleEvent<DisposeUrlLoadersEvent>(engine.CreateEntity("stub"));
            });

            if (NetworkService != null && NetworkService.Connected) {
                NetworkService.Disconnect();
            }

            DestroyAllGameObjects();
        }

        static void DestroyAllGameObjects() {
            Transform[] array = Object.FindObjectsOfType<Transform>();
            List<GameObject> list = new();

            for (int i = 0; i < array.Length; i++) {
                if (array[i].parent == null) {
                    list.Add(array[i].gameObject);
                }
            }

            for (int j = 0; j < list.Count; j++) {
                list[j].gameObject.SetActive(false);
            }

            for (int k = 0; k < list.Count; k++) {
                Object.Destroy(list[k]);
            }
        }
    }
}
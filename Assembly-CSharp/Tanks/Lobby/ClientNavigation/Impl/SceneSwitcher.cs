using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientResources.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.ClientLauncher.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public static class SceneSwitcher {
        [Inject] public static NetworkService NetworkService { get; set; }

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public static void CleanAndRestart() {
            ApplicationUtils.Restart();
        }

        public static void CleanAndSwitch(string sceneName) {
            CleanPreviousScene();
            SceneManager.LoadScene(sceneName);
        }

        public static void Clean() {
            AssetBundleDiskCache.Clean();
            AssetBundlesStorage.Clean();
            ServiceRegistry.Reset();
            InjectionUtils.ClearInjectionPoints(typeof(InjectAttribute));
            BaseElementCanvasScaler.MarkNeedInitialize();
            FatalErrorHandler.IsErrorScreenWasShown = false;
        }

        static void CleanPreviousScene() {
            InitConfigurationActivator.LauncherPassed = false;
            DisposeUrlLoaders();

            if (NetworkService != null && NetworkService.IsConnected) {
                NetworkService.Disconnect();
            }

            DestroyAllGameObjects();
        }

        public static void DisposeUrlLoaders() {
            EngineService.Engine.ScheduleEvent<DisposeUrlLoadersEvent>(EngineService.EntityStub);
        }

        static void DestroyAllGameObjects() {
            Transform[] array = Object.FindObjectsOfType<Transform>();
            List<GameObject> list = new();

            for (int i = 0; i < array.Length; i++) {
                if (array[i].parent == null && array[i].GetComponent<SkipRemoveOnSceneSwitch>() == null) {
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
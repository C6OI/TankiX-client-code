using System.Collections.Generic;
using System.Linq;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.Library.ClientUnityIntegration.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientResources.Impl {
    public class SceneFromBundleLoaderActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator {
        public GameObject progressBar;

        public AssetReference sceneListRef;

        bool instantiating;

        int loadingCount;

        readonly ILog logger;

        bool startedLoading;

        public SceneFromBundleLoaderActivator() => logger = LoggerProvider.GetLogger<SceneLoaderActivator>();

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        void Update() {
            SceneList sceneList = null;

            if (startedLoading) {
                IEnumerable<LoadedSceneNode> source = from n in EngineService.Engine.SelectAll<LoadedSceneNode>()
                                                      where n.sceneLoader.sceneName.Equals(GetInstanceID().ToString())
                                                      select n;

                if (source.Any()) {
                    sceneList = (SceneList)source.First().resourceData.Data;
                }
            }

            if (instantiating) {
                enabled = false;
                logger.Info("Complete");
                Complete();
            } else {
                if (!startedLoading || !(sceneList != null)) {
                    return;
                }

                logger.InfoFormat("Finished downloading scenes, instantiating...");
                instantiating = true;

                for (int i = 0; i < sceneList.scenes.Length; i++) {
                    if (sceneList.scenes[i].initAfterLoading) {
                        string sceneName = sceneList.scenes[i].sceneName;
                        logger.InfoFormat("LoadScene {0}", sceneName);
                        UnityUtil.LoadScene(sceneList.scenes[i].scene, sceneName, true);
                    }
                }
            }
        }

        public void RegisterSystemsAndTemplates() {
            EngineService.SystemRegistry.RegisterNode<LoadedSceneNode>();
            EngineService.SystemRegistry.RegisterNode<SceneLoaderNode>();
        }

        protected override void Activate() {
            StartLoading();
        }

        void StartLoading() {
            startedLoading = true;

            if (progressBar != null) {
                progressBar.SetActive(true);
            }

            Entity entity = EngineService.Engine.CreateEntity("ScenesLoader");
            entity.AddComponent(new AssetReferenceComponent(sceneListRef));

            entity.AddComponent(new AssetRequestComponent {
                AssetStoreLevel = AssetStoreLevel.MANAGED
            });

            entity.AddComponent(new SceneLoaderComponent {
                sceneName = string.Empty + GetInstanceID()
            });
        }

        public class SceneLoaderComponent : Component {
            public string sceneName;
        }

        public class LoadedSceneNode : Node {
            public ResourceDataComponent resourceData;
            public SceneLoaderComponent sceneLoader;
        }

        public class SceneLoaderNode : Node {
            public ResourceLoadStatComponent resourceLoadStat;
            public SceneLoaderComponent sceneLoader;
        }
    }
}
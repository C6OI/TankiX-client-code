using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class SoundLoaderWaitActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator {
        bool startedLoading;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        void Update() {
            if (startedLoading) {
                LoadedSoundNode loadedSoundNode = EngineService.Engine.SelectAll<LoadedSoundNode>().FirstOrDefault();

                if (loadedSoundNode != null) {
                    SoundListenerResourcesBehaviour component = ((GameObject)loadedSoundNode.resourceData.Data).GetComponent<SoundListenerResourcesBehaviour>();
                    loadedSoundNode.Entity.AddComponent(new SoundListenerResourcesComponent(component));
                    enabled = false;
                    Complete();
                }
            }
        }

        public void RegisterSystemsAndTemplates() {
            EngineService.SystemRegistry.RegisterNode<LoadedSoundNode>();
        }

        protected override void Activate() {
            startedLoading = true;
        }

        public class LoadedSoundNode : Node {
            public ResourceDataComponent resourceData;
            public SoundListenerComponent soundListener;
        }
    }
}
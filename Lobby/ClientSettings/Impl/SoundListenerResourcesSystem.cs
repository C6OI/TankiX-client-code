using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Lobby.ClientSettings.Impl {
    public class SoundListenerResourcesSystem : ECSSystem {
        [OnEventFire]
        public void StartLoadingSoundListenerResources(NodeAddedEvent e,
            SingleNode<SoundListenerResourcesReferenceComponent> listener) {
            listener.Entity.AddComponent(new AssetReferenceComponent(listener.component.Resources));
            listener.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void ConfigureSoundListenerWhenResourcesLoaded(NodeAddedEvent e,
            SoundListenerWithLoadedResourcesNode listener) {
            SoundListenerResourcesBehaviour component =
                ((GameObject)listener.resourceData.Data).GetComponent<SoundListenerResourcesBehaviour>();

            listener.Entity.AddComponent(new SoundListenerResourcesComponent(component));
        }

        public class SoundListenerWithLoadedResourcesNode : Node {
            public ResourceDataComponent resourceData;
            public SoundListenerComponent soundListener;
        }
    }
}
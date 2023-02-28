using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.Serialization;

namespace Tanks.Lobby.ClientSettings.API {
    public class SoundLoaderStartActivator : UnityAwareActivator<AutoCompleting> {
        [FormerlySerializedAs("sceneListRef")] public AssetReference audioResourcesRef;

        public EntityBehaviour entity;

        protected override void Activate() {
            entity.Entity.AddComponent(new AssetReferenceComponent(audioResourcesRef));

            entity.Entity.AddComponent(new AssetRequestComponent {
                AssetStoreLevel = AssetStoreLevel.MANAGED
            });
        }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class ConfigEntityLoaderSystem : ECSSystem {
        [Inject] public static ConfigEntityLoader EntityLoader { get; set; }

        [OnEventFire]
        public void LoadConfigEntity(ClientStartEvent e, Node node) {
            EntityLoader.LoadEntities(this);
        }
    }
}
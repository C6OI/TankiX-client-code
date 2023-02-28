using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1485852459997L)]
    public class ModuleGroupComponent : GroupComponent {
        public ModuleGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ModuleGroupComponent(long key)
            : base(key) { }
    }
}
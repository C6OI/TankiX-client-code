using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(635908808598551080L)]
    public class ParentGroupComponent : GroupComponent {
        public ParentGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ParentGroupComponent(long key)
            : base(key) { }
    }
}
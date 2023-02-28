using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1544510801819L)]
    public class FractionGroupComponent : GroupComponent {
        public FractionGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public FractionGroupComponent(long key)
            : base(key) { }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636177020058645390L)]
    public class SpecialOfferGroupComponent : GroupComponent {
        public SpecialOfferGroupComponent(Entity keyEntity)
            : this(keyEntity.Id) { }

        public SpecialOfferGroupComponent(long key)
            : base(key) { }
    }
}
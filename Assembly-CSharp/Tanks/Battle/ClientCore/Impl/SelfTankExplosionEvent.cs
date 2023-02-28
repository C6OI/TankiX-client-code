using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1447764683298L)]
    public class SelfTankExplosionEvent : Event {
        public bool CanDetachWeapon { get; set; }
    }
}
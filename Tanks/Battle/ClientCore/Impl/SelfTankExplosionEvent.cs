using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1447764683298L)]
    [Shared]
    public class SelfTankExplosionEvent : Event {
        public bool CanDetachWeapon { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    [Shared]
    [SerialVersionUID(-9112902007903352542L)]
    public class AnalogMoveCommandEvent : Event {
        [ProtocolOptional] public Movement? Movement { get; set; }

        [ProtocolOptional] public MoveControl? MoveControl { get; set; }

        [ProtocolOptional] public float? WeaponRotation { get; set; }

        [ProtocolOptional] public float? WeaponControl { get; set; }

        public void Init(ref Movement? movement, MoveControl? moveControl, float? weaponRotation, float? weaponControl) {
            Movement = movement;
            MoveControl = moveControl;
            WeaponRotation = weaponRotation;
            WeaponControl = weaponControl;
        }
    }
}
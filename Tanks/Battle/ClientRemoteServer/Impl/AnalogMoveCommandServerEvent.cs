using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    [Shared]
    [SerialVersionUID(2596682299194665575L)]
    public class AnalogMoveCommandServerEvent : Event {
        [ProtocolOptional] public Movement? Movement { get; set; }

        [ProtocolOptional] public MoveControl? MoveControl { get; set; }

        [ProtocolOptional] public float? WeaponRotation { get; set; }

        [ProtocolOptional] public float? WeaponControl { get; set; }
    }
}
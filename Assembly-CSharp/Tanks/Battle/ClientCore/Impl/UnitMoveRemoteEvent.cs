using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486036010735L)]
    [Shared]
    public class UnitMoveRemoteEvent : UnitMoveEvent {
        public UnitMoveRemoteEvent() { }

        public UnitMoveRemoteEvent(Movement move)
            : base(move) { }
    }
}
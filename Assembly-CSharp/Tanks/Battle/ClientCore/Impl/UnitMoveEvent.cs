using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485519185293L)]
    public class UnitMoveEvent : Event {
        public UnitMoveEvent(Movement move) => UnitMove = move;

        public UnitMoveEvent() { }

        public Movement UnitMove { get; set; }
    }
}
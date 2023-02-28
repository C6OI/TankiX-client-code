using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1504069546662L)]
    public class ElevatedAccessUserIncreaseScoreEvent : Event {
        public TeamColor TeamColor { get; set; }

        public int Amount { get; set; }
    }
}
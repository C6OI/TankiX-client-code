using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1515482797839L)]
    public class ElevatedAccessUserAddBotsToBattleEvent : Event {
        public int Count { get; set; }

        public TeamColor TeamColor { get; set; }
    }
}
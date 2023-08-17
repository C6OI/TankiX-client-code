using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635890723433891050L)]
    [Shared]
    public class RequestLoadBattleInfoEvent : Event {
        public RequestLoadBattleInfoEvent(long battleId) => BattleId = battleId;

        public long BattleId { get; private set; }
    }
}
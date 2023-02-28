using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(635890736905417870L)]
    public class BattleInfoForLabelLoadedEvent : Event {
        public Entity Map { get; set; }

        public long BattleId { get; set; }

        public string BattleMode { get; set; }
    }
}
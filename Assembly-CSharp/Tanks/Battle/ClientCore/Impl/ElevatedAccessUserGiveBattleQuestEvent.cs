using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1518092856552L)]
    public class ElevatedAccessUserGiveBattleQuestEvent : Event {
        public string QuestPath { get; set; }
    }
}
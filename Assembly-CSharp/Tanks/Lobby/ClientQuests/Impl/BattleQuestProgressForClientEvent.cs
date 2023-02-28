using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    [Shared]
    [SerialVersionUID(1516708874681L)]
    public class BattleQuestProgressForClientEvent : Event {
        public int ProgressDelta { get; set; }
    }
}
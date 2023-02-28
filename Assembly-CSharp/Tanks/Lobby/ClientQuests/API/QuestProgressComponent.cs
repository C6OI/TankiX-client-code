using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1476091404409L)]
    public class QuestProgressComponent : Component {
        public float PrevValue { get; set; }

        public float CurrentValue { get; set; }

        public float TargetValue { get; set; }

        public bool PrevComplete { get; set; }

        public bool CurrentComplete { get; set; }
    }
}
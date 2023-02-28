using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1508318283371L)]
    public class RoundReputationEarnedEvent : Event {
        public int Delta { get; set; }

        public bool UnfairMatching { get; set; }
    }
}
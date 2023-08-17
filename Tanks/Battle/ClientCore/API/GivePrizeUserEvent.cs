using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1455093130206L)]
    public class GivePrizeUserEvent : Event {
        public int Prize { get; set; }
    }
}
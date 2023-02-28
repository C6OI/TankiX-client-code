using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-1937089974629265090L)]
    public class SelfHammerShotEvent : SelfShotEvent {
        public int RandomSeed { get; set; }
    }
}
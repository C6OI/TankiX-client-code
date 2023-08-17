using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-1937089974629265090L)]
    [Shared]
    public class SelfHammerShotEvent : SelfShotEvent {
        public int RandomSeed { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class SelfDestructionMessageComponent : Component {
        public string Message { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class AutokickMessageComponent : Component {
        public string Message { get; set; }
    }
}
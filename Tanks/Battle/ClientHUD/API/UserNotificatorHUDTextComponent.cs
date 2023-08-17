using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class UserNotificatorHUDTextComponent : Component {
        public string UserRankMessageFormat { get; set; }
    }
}
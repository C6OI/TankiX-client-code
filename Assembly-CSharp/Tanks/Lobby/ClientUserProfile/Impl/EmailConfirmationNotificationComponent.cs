using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class EmailConfirmationNotificationComponent : Component {
        public string ConfirmationMessageTemplate { get; set; }

        public string ChangeEmailMessageTemplate { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1523254126051L)]
    public class EmailConfirmationCodeConfigComponent : Component {
        public long EmailSendThresholdInSeconds { get; set; }

        public long ConfirmationCodeInputMaxLength { get; set; }
    }
}
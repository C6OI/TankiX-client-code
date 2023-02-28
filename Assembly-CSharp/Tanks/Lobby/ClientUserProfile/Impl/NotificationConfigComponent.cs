using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(635999625309067160L)]
    public class NotificationConfigComponent : Component {
        public float ShowDuration { get; set; }

        public float ShowDelay { get; set; }

        public int Order { get; set; }

        public bool IsFullScreen { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1479820450460L)]
    public class WebIdComponent : Component {
        public string WebId { get; set; }

        public string Utm { get; set; }

        public string GoogleAnalyticsId { get; set; }

        public string WebIdUid { get; set; }
    }
}
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNotifications.API {
    [Shared]
    [SerialVersionUID(636564551794907676L)]
    public class ReleaseGiftsNotificationComponent : Component {
        public Dictionary<long, int> Reward { get; set; }
    }
}
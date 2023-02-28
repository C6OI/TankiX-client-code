using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1499174753575L)]
    public class UserStatisticsComponent : Component {
        public Dictionary<string, long> Statistics { get; set; }
    }
}
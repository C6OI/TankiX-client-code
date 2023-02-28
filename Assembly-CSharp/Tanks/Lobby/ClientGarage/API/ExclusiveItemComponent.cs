using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1474277994422L)]
    public class ExclusiveItemComponent : Component {
        public List<Publisher> ForbiddenForPublishers { get; set; }
    }
}
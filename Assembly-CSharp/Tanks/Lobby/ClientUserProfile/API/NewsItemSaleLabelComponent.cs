using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1481290407948L)]
    public class NewsItemSaleLabelComponent : Component {
        public string Text { get; set; }
    }
}
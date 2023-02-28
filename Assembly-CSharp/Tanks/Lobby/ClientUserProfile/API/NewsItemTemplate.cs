using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1479374682015L)]
    public interface NewsItemTemplate : Template {
        NewsItemComponent newsItem();

        NewsItemSaleLabelComponent newsItemSaleLabel();
    }
}
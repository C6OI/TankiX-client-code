using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1467631070678L)]
    public interface SkinUserItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        SkinItemTemplate, UserItemTemplate { }
}
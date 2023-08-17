using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607958560L)]
    public interface HullSkinUserItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        HullSkinItemTemplate, SkinItemTemplate, SkinUserItemTemplate, UserItemTemplate { }
}
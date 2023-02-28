using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607958560L)]
    public interface HullSkinUserItemTemplate : HullSkinItemTemplate, SkinUserItemTemplate, Template, SkinItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate,
        ItemImagedTemplate { }
}
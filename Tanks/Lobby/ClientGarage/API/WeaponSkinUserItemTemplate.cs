using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607756132L)]
    public interface WeaponSkinUserItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        SkinItemTemplate, SkinUserItemTemplate, WeaponSkinItemTemplate, UserItemTemplate { }
}
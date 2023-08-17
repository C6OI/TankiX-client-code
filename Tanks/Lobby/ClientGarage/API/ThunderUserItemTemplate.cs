using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435139182866L)]
    public interface ThunderUserItemTemplate : Template, GarageItemTemplate, UpgradableUserItemTemplate, UserItemTemplate,
        ThunderItemTemplate, WeaponItemTemplate { }
}
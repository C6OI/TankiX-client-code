using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138575888L)]
    public interface RailgunUserItemTemplate : Template, GarageItemTemplate, UpgradableUserItemTemplate, UserItemTemplate,
        RailgunItemTemplate, WeaponItemTemplate { }
}
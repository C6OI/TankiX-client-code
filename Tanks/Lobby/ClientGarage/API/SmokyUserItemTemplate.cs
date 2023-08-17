using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433406776150L)]
    public interface SmokyUserItemTemplate : Template, GarageItemTemplate, UpgradableUserItemTemplate, UserItemTemplate,
        SmokyItemTemplate, WeaponItemTemplate { }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435138823086L)]
    public interface HammerUserItemTemplate : Template, GarageItemTemplate, UpgradableUserItemTemplate, UserItemTemplate,
        HammerItemTemplate, WeaponItemTemplate { }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435910167704L)]
    public interface IsisUserItemTemplate : Template, GarageItemTemplate, UpgradableUserItemTemplate, UserItemTemplate,
        IsisItemTemplate, WeaponItemTemplate { }
}
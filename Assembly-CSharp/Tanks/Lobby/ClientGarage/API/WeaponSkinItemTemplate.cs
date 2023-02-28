using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607472330L)]
    public interface WeaponSkinItemTemplate : Template {
        [AutoAdded]
        WeaponSkinItemComponent weaponSkinItem();
    }
}
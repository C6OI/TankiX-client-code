using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714683461L)]
    public interface FreezeItemTemplete : WeaponItemTemplate, GarageItemTemplate, Template { }
}
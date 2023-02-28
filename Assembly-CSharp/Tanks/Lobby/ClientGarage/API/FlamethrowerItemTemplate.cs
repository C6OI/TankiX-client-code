using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714640358L)]
    public interface FlamethrowerItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
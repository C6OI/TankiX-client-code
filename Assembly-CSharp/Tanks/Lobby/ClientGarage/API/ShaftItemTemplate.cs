using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713831380L)]
    public interface ShaftItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
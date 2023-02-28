using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714465485L)]
    public interface RailgunItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
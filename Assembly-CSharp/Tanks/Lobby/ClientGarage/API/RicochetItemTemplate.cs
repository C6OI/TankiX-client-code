using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714406079L)]
    public interface RicochetItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
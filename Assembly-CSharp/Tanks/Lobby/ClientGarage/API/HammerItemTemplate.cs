using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714587857L)]
    public interface HammerItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
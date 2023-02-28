using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714765888L)]
    public interface VulcanItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714721313L)]
    public interface IsisItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
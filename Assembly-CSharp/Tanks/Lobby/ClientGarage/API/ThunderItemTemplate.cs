using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713786006L)]
    public interface ThunderItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
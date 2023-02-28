using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713726440L)]
    public interface TwinsItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
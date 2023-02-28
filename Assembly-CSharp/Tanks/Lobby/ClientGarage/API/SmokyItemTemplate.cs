using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437710550541L)]
    public interface SmokyItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template { }
}
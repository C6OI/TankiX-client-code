using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1433419930410L)]
    public interface GarageItemImagedTemplate : GarageItemTemplate, ItemImagedTemplate, Template { }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1493975927116L)]
    public interface PresetItemTemplate : GarageItemTemplate, Template {
        [AutoAdded]
        PresetItemComponent presetItem();
    }
}
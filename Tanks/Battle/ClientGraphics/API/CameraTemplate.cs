using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(1453364101465L)]
    public interface CameraTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        CameraOffsetConfigComponent cameraOffsetConfig();
    }
}
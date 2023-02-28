using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientUnityIntegration.API {
    [SerialVersionUID(636046057939422722L)]
    public interface ConfigPathCollectionTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ConfigPathCollectionComponent configPathCollection();
    }
}
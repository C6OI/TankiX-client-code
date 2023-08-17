using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Lobby.ClientLoading.API {
    [SerialVersionUID(7567231643092L)]
    public interface WarmupResourcesTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        WarmupResourcesComponent warmupResources();
    }
}
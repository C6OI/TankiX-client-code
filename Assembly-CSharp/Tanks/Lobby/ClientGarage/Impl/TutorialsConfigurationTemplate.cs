using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1503997441606L)]
    public interface TutorialsConfigurationTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        TutorialConfigurationComponent tutorialConfiguration();
    }
}
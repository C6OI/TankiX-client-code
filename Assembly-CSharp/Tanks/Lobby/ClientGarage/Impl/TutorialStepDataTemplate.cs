using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1504002856142L)]
    public interface TutorialStepDataTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        TutorialStepDataComponent tutorialStepData();
    }
}
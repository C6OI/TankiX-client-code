using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1504000056991L)]
    public interface TutorialDataTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        TutorialDataComponent tutorialData();

        [AutoAdded]
        [PersistentConfig]
        TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials();

        [AutoAdded]
        TutorialGroupComponent tutorialGroup();
    }
}
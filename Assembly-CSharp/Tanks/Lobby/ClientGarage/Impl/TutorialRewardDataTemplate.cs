using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1509340416220L)]
    public interface TutorialRewardDataTemplate : TutorialStepDataTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        TutorialRewardDataComponent tutorialRewardData();
    }
}
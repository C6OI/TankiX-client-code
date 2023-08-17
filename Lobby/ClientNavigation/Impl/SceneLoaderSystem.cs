using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientNavigation.Impl {
    public class SceneLoaderSystem : ECSSystem {
        [OnEventFire]
        public void LoadScene(LoadSceneEvent e, Node newSceneNode,
            [JoinAll] Optional<SingleNode<CurrentSceneComponent>> currentScene) {
            if (currentScene.IsPresent()) {
                currentScene.Get().Entity.RemoveComponent<CurrentSceneComponent>();
            }

            UnityUtil.LoadSceneAsync(e.SceneAsset, e.SceneName);
            newSceneNode.Entity.AddComponent<CurrentSceneComponent>();
        }

        [OnEventComplete]
        public void SwitchToEntrance(SwitchToEntranceSceneEvent e, Node node) =>
            SceneSwitcher.CleanAndSwitch(SceneNames.ENTRANCE);
    }
}
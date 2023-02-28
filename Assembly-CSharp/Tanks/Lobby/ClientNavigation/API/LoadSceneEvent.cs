using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientNavigation.API {
    public class LoadSceneEvent : Event {
        public LoadSceneEvent(string sceneName, Object sceneAsset) {
            SceneName = sceneName;
            SceneAsset = sceneAsset;
        }

        public string SceneName { get; }

        public Object SceneAsset { get; }
    }
}
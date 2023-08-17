using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352545785226L)]
    public class MapInstanceComponent : Component {
        public MapInstanceComponent() { }

        public MapInstanceComponent(GameObject sceneRoot) => SceneRoot = sceneRoot;

        public GameObject SceneRoot { get; set; }
    }
}
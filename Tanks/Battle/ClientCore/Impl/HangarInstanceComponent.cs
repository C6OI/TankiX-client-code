using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635838815918272155L)]
    public class HangarInstanceComponent : Component {
        public HangarInstanceComponent() { }

        public HangarInstanceComponent(GameObject sceneRoot) => SceneRoot = sceneRoot;

        public GameObject SceneRoot { get; set; }
    }
}
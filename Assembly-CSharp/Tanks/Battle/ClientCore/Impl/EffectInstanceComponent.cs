using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class EffectInstanceComponent : Component {
        public EffectInstanceComponent(GameObject gameObject) {
            GameObject = gameObject;
            Object.DontDestroyOnLoad(gameObject);
        }

        public GameObject GameObject { get; set; }
    }
}
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseEjectorComponent : MonoBehaviour, Component {
        public GameObject casePrefab;

        public float initialAngularSpeed;

        public float initialSpeed;
    }
}
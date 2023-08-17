using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MuzzleFlashComponent : MonoBehaviour, Component {
        public GameObject muzzleFlashPrefab;

        public float duration = 0.5f;
    }
}
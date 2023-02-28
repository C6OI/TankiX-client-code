using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class BonusRegionClientConfigComponent : MonoBehaviour, Component {
        public float maxOpacityRadius = 20f;

        public float minOpacityRadius = 30f;

        public float opacityChangingSpeed = 1f;
    }
}
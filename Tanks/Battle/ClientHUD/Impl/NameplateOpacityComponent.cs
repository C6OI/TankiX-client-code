using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateOpacityComponent : MonoBehaviour, Component {
        public float sqrConcealmentDistance;
    }
}
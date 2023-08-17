using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BulletEffectInstanceComponent : Component {
        public GameObject Effect { get; set; }
    }
}
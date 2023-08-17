using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankActiveTextureBehaviourComponent : MonoBehaviour, Component {
        public Texture2D tankActiveTexture;
    }
}
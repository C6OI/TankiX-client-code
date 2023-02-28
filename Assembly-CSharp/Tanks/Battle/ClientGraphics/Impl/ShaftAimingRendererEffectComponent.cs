using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingRendererEffectComponent : MonoBehaviour, Component {
        [SerializeField] int transparentRenderQueue = 3000;

        [SerializeField] float alphaRecoveringSpeed = 2f;

        public float AlphaRecoveringSpeed {
            get => alphaRecoveringSpeed;
            set => alphaRecoveringSpeed = value;
        }

        public int TransparentRenderQueue {
            get => transparentRenderQueue;
            set => transparentRenderQueue = value;
        }
    }
}
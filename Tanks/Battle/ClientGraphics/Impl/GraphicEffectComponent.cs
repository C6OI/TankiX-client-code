using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GraphicEffectComponent : Component {
        public GraphicEffectComponent() { }

        public GraphicEffectComponent(GameObject effectObject) => EffectObject = effectObject;

        public GameObject EffectObject { get; set; }
    }
}
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapHidingGeometryComponent : Component {
        public Renderer[] hidingRenderers;

        public MapHidingGeometryComponent() { }

        public MapHidingGeometryComponent(Renderer[] hidingRenderers) => this.hidingRenderers = hidingRenderers;
    }
}
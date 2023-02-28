using System.Collections.Generic;
using LeopotamGroup.Pooling;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MainPoolContainerComponent : Component {
        public readonly Dictionary<GameObject, PoolContainer> PrefabToPoolDict = new();
        public Transform MainContainerTransform;
    }
}
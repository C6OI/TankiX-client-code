using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class HangarAsyncLoadComponent : Component {
        public AsyncOperation AsyncOperation { get; set; }
    }
}
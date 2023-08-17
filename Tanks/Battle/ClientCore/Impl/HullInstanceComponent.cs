using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635824352866675226L)]
    public class HullInstanceComponent : Component {
        public GameObject HullInstance { get; set; }
    }
}
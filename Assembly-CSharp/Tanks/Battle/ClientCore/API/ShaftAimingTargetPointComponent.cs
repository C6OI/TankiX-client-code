using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(8445798616771064825L)]
    public class ShaftAimingTargetPointComponent : SharedChangeableComponent {
        [ProtocolOptional] public Vector3 Point { get; set; }

        public bool IsInsideTankPart { get; set; }
    }
}
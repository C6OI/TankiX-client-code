using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1508220592078L)]
    public class AutopilotWeaponControllerComponent : SharedChangeableComponent {
        public bool Attack { get; set; }

        public bool TragerAchievable { get; set; }

        [ProtocolOptional] public Entity Target { get; set; }

        public float Accurasy { get; set; }

        [ProtocolTransient] public bool Fire { get; set; }

        [ProtocolTransient] public bool ShouldMiss { get; set; }

        [ProtocolTransient] public Rigidbody TargetRigidbody { get; set; }

        [ProtocolTransient] public bool IsOnShootingLine { get; set; }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(5440037691022467911L)]
    public class SelfShotEvent : BaseShotEvent {
        public SelfShotEvent() { }

        public SelfShotEvent(Vector3 shotDirection)
            : base(shotDirection) { }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(5440037691022467911L)]
    [Shared]
    public class SelfShotEvent : BaseShotEvent {
        public SelfShotEvent() { }

        public SelfShotEvent(Vector3 shotDirection)
            : base(shotDirection) { }
    }
}
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftAimingShotPrepareEvent : BaseShotPrepareEvent {
        public ShaftAimingShotPrepareEvent() { }

        public ShaftAimingShotPrepareEvent(Vector3 workingDir) => WorkingDir = workingDir;

        public Vector3 WorkingDir { get; set; }
    }
}
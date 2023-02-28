using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public abstract class TankPartVisualRootComponent : MonoBehaviour {
        [SerializeField] VisualTriggerMarkerComponent visualTriggerMarker;

        public VisualTriggerMarkerComponent VisualTriggerMarker => visualTriggerMarker;
    }
}
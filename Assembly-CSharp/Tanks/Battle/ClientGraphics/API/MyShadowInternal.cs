using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class MyShadowInternal : MonoBehaviour {
        public Projector Projector { get; set; }

        public Material CasterMaterial { get; set; }

        public float BaseAlpha { get; set; }

        public Bounds ProjectionBoundInLightSpace { get; set; }
    }
}
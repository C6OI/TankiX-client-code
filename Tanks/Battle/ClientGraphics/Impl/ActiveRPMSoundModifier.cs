using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ActiveRPMSoundModifier : AbstractRPMSoundModifier {
        public override bool CheckLoad(float smoothedLoad) => smoothedLoad > 0f;

        public override float CalculateLoadPartForModifier(float smoothedLoad) => Mathf.Sqrt(smoothedLoad);
    }
}
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class NormalRPMSoundModifier : AbstractRPMSoundModifier {
        public override bool CheckLoad(float smoothedLoad) => smoothedLoad < 1f;

        public override float CalculateLoadPartForModifier(float smoothedLoad) => Mathf.Sqrt(1f - smoothedLoad);
    }
}
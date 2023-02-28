using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ActiveRPMSoundModifier : AbstractRPMSoundModifier {
        [SerializeField] AudioMixerGroup selfActiveGroup;

        [SerializeField] AudioMixerGroup remoteActiveGroup;

        protected override void Awake() {
            base.Awake();
            Source.outputAudioMixerGroup = !RpmSoundBehaviour.HullSoundEngine.SelfEngine ? remoteActiveGroup : selfActiveGroup;
        }

        public override bool CheckLoad(float smoothedLoad) => smoothedLoad > 0f;

        public override float CalculateLoadPartForModifier(float smoothedLoad) => Mathf.Sqrt(CalculateLinearLoadModifier(smoothedLoad));
    }
}
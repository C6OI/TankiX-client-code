using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RPMSoundBehaviour : MonoBehaviour {
        [SerializeField] float rpm;

        [SerializeField] ActiveRPMSoundModifier activeRPMSound;

        [SerializeField] NormalRPMSoundModifier normalRPMSound;

        [SerializeField] float rangeBeginRPM;

        [SerializeField] float rangeEndRPM;

        [SerializeField] HullSoundEngineController hullSoundEngine;

        public HullSoundEngineController HullSoundEngine => hullSoundEngine;

        public float RPM => rpm;

        public float RangeBeginRpm => rangeBeginRPM;

        public float RangeEndRpm => rangeEndRPM;

        public bool NeedToStop => activeRPMSound.NeedToStop && normalRPMSound.NeedToStop;

        public void Build(HullSoundEngineController engine, float prevRPM, float nextRPM, float blendRange) {
            hullSoundEngine = engine;
            rangeBeginRPM = Mathf.Lerp(rpm, prevRPM, blendRange);
            rangeEndRPM = Mathf.Lerp(rpm, nextRPM, blendRange);
            activeRPMSound.Build(this);
            normalRPMSound.Build(this);
        }

        public void Play(float volume) {
            activeRPMSound.Play(volume);
            normalRPMSound.Play(volume);
        }

        public void Stop() {
            activeRPMSound.Stop();
            normalRPMSound.Stop();
        }
    }
}
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DoubleDamageSoundEffectComponent : BaseEffectSoundComponent<SoundController> {
        [SerializeField] float startSoundDelaySec;

        [SerializeField] float stopSoundDelaySec;

        [SerializeField] float startSoundOffsetSec;

        [SerializeField] float stopSoundOffsetSec;

        public float StartSoundDelaySec {
            get => startSoundDelaySec;
            set => startSoundDelaySec = value;
        }

        public float StopSoundDelaySec {
            get => stopSoundDelaySec;
            set => stopSoundDelaySec = value;
        }

        public float StartSoundOffsetSec {
            get => startSoundOffsetSec;
            set => startSoundOffsetSec = value;
        }

        public float StopSoundOffsetSec {
            get => stopSoundOffsetSec;
            set => stopSoundOffsetSec = value;
        }

        public override void BeginEffect() {
            StopSound.StopImmediately();
            StartSound.SetSoundActive();
        }

        public override void StopEffect() {
            StartSound.StopImmediately();
            StopSound.SetSoundActive();
        }

        public void RecalculatePlayingParameters() {
            SoundController startSound = StartSound;
            SoundController stopSound = StopSound;
            startSound.PlayingDelaySec = StartSoundDelaySec;
            stopSound.PlayingDelaySec = StopSoundDelaySec;
            startSound.PlayingOffsetSec = StartSoundOffsetSec;
            stopSound.PlayingOffsetSec = StopSoundOffsetSec;
        }
    }
}
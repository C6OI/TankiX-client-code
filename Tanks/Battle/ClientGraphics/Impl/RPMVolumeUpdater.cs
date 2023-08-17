using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RPMVolumeUpdater : AbstractRPMSoundUpdater {
        [SerializeField] RPMVolumeUpdaterFinishBehaviour rpmVolumeUpdaterFinishBehaviour;

        void Update() => UpdateVolume();

        protected override void OnEnable() {
            UpdateVolume();
            rpmVolumeUpdaterFinishBehaviour.enabled = false;
            AudioSource source = parentModifier.Source;

            if (!source.isPlaying) {
                source.Play();
            }
        }

        protected override void OnDisable() {
            if (alive) {
                rpmVolumeUpdaterFinishBehaviour.enabled = true;
            }
        }

        void UpdateVolume() {
            AudioSource source = parentModifier.Source;
            float engineRpm = engine.EngineRpm;
            float engineLoad = engine.EngineLoad;
            float rpmSoundVolume = parentModifier.RpmSoundVolume;

            if (!engine.IsRPMWithinRange(rpmSoundBehaviour, engine.EngineRpm)) {
                source.volume = 0f;
                parentModifier.NeedToStop = true;
            } else if (!parentModifier.CheckLoad(engine.EngineLoad)) {
                source.volume = 0f;
            } else {
                float volume = rpmSoundVolume * parentModifier.CalculateModifier(engineRpm, engineLoad);
                source.volume = volume;
            }
        }

        public override void Build(HullSoundEngineController engine, AbstractRPMSoundModifier abstractRPMSoundModifier,
            RPMSoundBehaviour rpmSoundBehaviour) {
            base.Build(engine, abstractRPMSoundModifier, rpmSoundBehaviour);
            rpmVolumeUpdaterFinishBehaviour = gameObject.AddComponent<RPMVolumeUpdaterFinishBehaviour>();
            rpmVolumeUpdaterFinishBehaviour.Build(parentModifier.Source);
        }
    }
}
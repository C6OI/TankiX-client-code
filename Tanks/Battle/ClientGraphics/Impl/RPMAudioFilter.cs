using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RPMAudioFilter : AbstractRPMSoundUpdater {
        float previousEngineLoad;
        float previousEngineRPM;

        void OnAudioFilterRead(float[] data, int channels) {
            int num = data.Length;
            bool flag = true;

            for (int i = 0; i < num; i++) {
                float t = i / (float)num;
                float num2 = Mathf.Lerp(previousEngineRPM, engine.EngineRpm, t);
                float smoothedLoad = Mathf.Lerp(previousEngineLoad, engine.EngineLoad, t);

                if (!engine.IsRPMWithinRange(rpmSoundBehaviour, num2)) {
                    data[i] = 0f;
                    flag = flag && true;
                } else if (!parentModifier.CheckLoad(smoothedLoad)) {
                    data[i] = 0f;
                    flag = false;
                } else {
                    data[i] *= parentModifier.RpmSoundVolume;
                    data[i] *= parentModifier.CalculateModifier(num2, smoothedLoad);
                    flag = false;
                }
            }

            parentModifier.NeedToStop = flag;
            UpdatePreviousParameters();
        }

        void UpdatePreviousParameters() {
            previousEngineLoad = engine.EngineLoad;
            previousEngineRPM = engine.EngineRpm;
        }

        public override void Play() {
            if (!parentModifier.Source.isPlaying) {
                UpdatePreviousParameters();
            }

            base.Play();
        }
    }
}
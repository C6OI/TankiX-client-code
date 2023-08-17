using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AbstractRPMSoundModifier : MonoBehaviour {
        [SerializeField] AudioSource source;

        [SerializeField] RPMSoundBehaviour rpmSoundBehaviour;

        [SerializeField] AbstractRPMSoundUpdater childUpdater;

        [SerializeField] float targetRPM;

        [SerializeField] bool needToStop;

        [SerializeField] float rpmSoundVolume;

        public AudioSource Source => source;

        public float RpmSoundVolume => Mathf.Min(rpmSoundVolume, 1f);

        public bool NeedToStop {
            get => needToStop;
            set => needToStop = value;
        }

        void Awake() {
            source.Stop();
            needToStop = false;
            rpmSoundVolume = 1f;
        }

        float CalculateRPMRangeFactor(float currentRPM) {
            float rangeBeginRpm = rpmSoundBehaviour.RangeBeginRpm;
            float rangeEndRpm = rpmSoundBehaviour.RangeEndRpm;
            return (currentRPM - rangeBeginRpm) / (rangeEndRpm - rangeBeginRpm);
        }

        void InitChildUpdater<TAdd, TRemove>() where TAdd : AbstractRPMSoundUpdater where TRemove : AbstractRPMSoundUpdater {
            TRemove component = gameObject.GetComponent<TRemove>();

            if (component != null) {
                DestroyImmediate(component);
            }

            childUpdater = gameObject.GetComponent<TAdd>();

            if (childUpdater == null) {
                childUpdater = gameObject.AddComponent<TAdd>();
            }
        }

        public float CalculateModifier(float smoothedRPM, float smoothedLoad) {
            bool flag = smoothedRPM >= targetRPM;
            float num = CalculateRPMRangeFactor(smoothedRPM);
            float num2 = CalculateLoadPartForModifier(smoothedLoad);
            float num3 = !flag ? Mathf.Sqrt(num) : Mathf.Sqrt(1f - num);
            return num3 * num2;
        }

        public void Build(RPMSoundBehaviour rpmSoundBehaviour) {
            this.rpmSoundBehaviour = rpmSoundBehaviour;
            targetRPM = rpmSoundBehaviour.RPM;
            HullSoundEngineController hullSoundEngine = rpmSoundBehaviour.HullSoundEngine;

            if (hullSoundEngine.UseAudioFilters) {
                InitChildUpdater<RPMAudioFilter, RPMVolumeUpdater>();
            } else {
                InitChildUpdater<RPMVolumeUpdater, RPMAudioFilter>();
            }

            childUpdater.Build(hullSoundEngine, this, rpmSoundBehaviour);
        }

        public void Play(float volume) {
            rpmSoundVolume = volume;
            needToStop = false;
            childUpdater.Play();
        }

        public void Stop() {
            needToStop = false;
            childUpdater.Stop();
        }

        public abstract bool CheckLoad(float smoothedLoad);

        public abstract float CalculateLoadPartForModifier(float smoothedLoad);
    }
}
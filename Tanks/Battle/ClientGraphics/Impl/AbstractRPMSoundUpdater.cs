using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AbstractRPMSoundUpdater : MonoBehaviour {
        [SerializeField] protected bool alive;

        [SerializeField] protected HullSoundEngineController engine;

        [SerializeField] protected AbstractRPMSoundModifier parentModifier;

        [SerializeField] protected RPMSoundBehaviour rpmSoundBehaviour;

        protected virtual void Awake() {
            Stop();
            alive = true;
        }

        protected virtual void OnEnable() => parentModifier.Source.Play();

        protected virtual void OnDisable() {
            if (alive) {
                parentModifier.Source.Pause();
            }
        }

        void OnApplicationQuit() => alive = false;

        public virtual void Build(HullSoundEngineController engine, AbstractRPMSoundModifier abstractRPMSoundModifier,
            RPMSoundBehaviour rpmSoundBehaviour) {
            RPMVolumeUpdaterFinishBehaviour component = gameObject.GetComponent<RPMVolumeUpdaterFinishBehaviour>();

            if (component != null) {
                DestroyImmediate(component);
            }

            this.engine = engine;
            parentModifier = abstractRPMSoundModifier;
            this.rpmSoundBehaviour = rpmSoundBehaviour;
        }

        public virtual void Play() => enabled = true;

        public virtual void Stop() => enabled = false;
    }
}
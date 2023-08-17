using UnityEngine;

namespace Lobby.ClientControls.API {
    public abstract class UISoundEffectController : MonoBehaviour {
        const string UI_SOUND_ROOT_NAME = "UISoundRoot";

        const string SOUND_NAME_POSTFIX = "Sound";

        static Transform uiTransformRoot;

        [SerializeField] AudioClip clip;

        [SerializeField] AudioSource sourceAsset;

        protected bool alive;

        AudioSource sourceInstance;

        static Transform UITransformRoot {
            get {
                if (uiTransformRoot == null) {
                    uiTransformRoot = new GameObject("UISoundRoot").transform;
                    uiTransformRoot.position = Vector3.zero;
                    uiTransformRoot.rotation = Quaternion.identity;
                }

                return uiTransformRoot;
            }
        }

        public abstract string HandlerName { get; }

        void Awake() {
            alive = true;
            sourceInstance = Instantiate(sourceAsset);
            GameObject gameObject = sourceInstance.gameObject;
            Transform transform = sourceInstance.transform;
            transform.parent = UITransformRoot;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.name = string.Format("{0}_{1}_{2}", this.gameObject.name, HandlerName, "Sound");
            sourceInstance.clip = clip;
        }

        void OnDestroy() {
            if (alive && (bool)sourceInstance) {
                Destroy(sourceInstance.gameObject);
            }
        }

        void OnApplicationQuit() => alive = false;

        public void PlaySoundEffect() {
            sourceInstance.Stop();
            sourceInstance.Play();
        }
    }
}
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseEffectSoundComponent<T> : MonoBehaviour, Component where T : UnityEngine.Component {
        [SerializeField] GameObject startSoundAsset;

        [SerializeField] GameObject stopSoundAsset;

        public GameObject StartSoundAsset {
            get => startSoundAsset;
            set => startSoundAsset = value;
        }

        public GameObject StopSoundAsset {
            get => stopSoundAsset;
            set => stopSoundAsset = value;
        }

        public T StartSound { get; set; }

        public T StopSound { get; set; }

        public abstract void BeginEffect();

        public abstract void StopEffect();

        public void Init(Transform root) {
            StartSound = Init(StartSoundAsset, root);
            StopSound = Init(StopSoundAsset, root);
        }

        T Init(GameObject go, Transform root) {
            GameObject gameObject = Instantiate(go);
            gameObject.transform.parent = root;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            return gameObject.GetComponent<T>();
        }
    }
}
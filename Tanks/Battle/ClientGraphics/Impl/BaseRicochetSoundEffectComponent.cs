using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseRicochetSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] AudioSource assetSource;

        [SerializeField] float lifetime = 2f;

        public void PlayEffect(Vector3 position) {
            AudioSource audioSource = Instantiate(assetSource, position, Quaternion.identity);
            Play(audioSource);
            Destroy(audioSource.gameObject, lifetime);
        }

        public abstract void Play(AudioSource sourceInstane);
    }
}
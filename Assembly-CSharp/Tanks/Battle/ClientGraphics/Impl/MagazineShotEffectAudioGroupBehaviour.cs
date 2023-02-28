using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MagazineShotEffectAudioGroupBehaviour : MonoBehaviour {
        [SerializeField] AudioMixerGroup selfShotGroup;

        [SerializeField] AudioMixerGroup remoteShotGroup;

        public AudioMixerGroup SelfShotGroup => selfShotGroup;

        public AudioMixerGroup RemoteShotGroup => remoteShotGroup;
    }
}
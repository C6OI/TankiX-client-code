using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseStreamHitWeaponSoundEffect : BehaviourComponent {
        [SerializeField] GameObject effectPrefab;

        public SoundController SoundController { get; set; }

        public GameObject EffectPrefab => effectPrefab;
    }
}
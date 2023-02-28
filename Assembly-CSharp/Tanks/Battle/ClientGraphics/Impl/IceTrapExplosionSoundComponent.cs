using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IceTrapExplosionSoundComponent : BehaviourComponent {
        [SerializeField] GameObject explosionSoundAsset;

        [SerializeField] float lifetime = 7f;

        public float Lifetime => lifetime;

        public GameObject ExplosionSoundAsset => explosionSoundAsset;
    }
}
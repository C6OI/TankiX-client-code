using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShootingEnergyFeedbackSoundComponent : BehaviourComponent {
        [SerializeField] WeaponFeedbackSoundBehaviour lowEnergyFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour LowEnergyFeedbackSoundAsset => lowEnergyFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour Instance { get; set; }
    }
}
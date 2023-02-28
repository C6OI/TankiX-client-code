using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitFeedbackSoundsComponent : BehaviourComponent {
        [SerializeField] WeaponFeedbackSoundBehaviour hammerHitFeedbackSoundAsset;

        [SerializeField] WeaponFeedbackSoundBehaviour smokyHitFeedbackSoundAsset;

        [SerializeField] WeaponFeedbackSoundBehaviour thunderHitFeedbackSoundAsset;

        [SerializeField] WeaponFeedbackSoundBehaviour railgunHitFeedbackSoundAsset;

        [SerializeField] WeaponFeedbackSoundBehaviour ricochetHitFeedbackSoundAsset;

        [SerializeField] WeaponFeedbackSoundBehaviour shaftHitFeedbackSoundAsset;

        [SerializeField] SoundController isisHealingFeedbackController;

        [SerializeField] SoundController isisAttackFeedbackController;

        [SerializeField] SoundController freezeWeaponAttackController;

        [SerializeField] SoundController flamethrowerWeaponAttackController;

        public WeaponFeedbackSoundBehaviour SmokyHitFeedbackSoundAsset => smokyHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour ThunderHitFeedbackSoundAsset => thunderHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour RailgunHitFeedbackSoundAsset => railgunHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour RicochetHitFeedbackSoundAsset => ricochetHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour ShaftHitFeedbackSoundAsset => shaftHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour HammerHitFeedbackSoundAsset => hammerHitFeedbackSoundAsset;

        public SoundController IsisHealingFeedbackController => isisHealingFeedbackController;

        public SoundController IsisAttackFeedbackController => isisAttackFeedbackController;

        public SoundController FreezeWeaponAttackController => freezeWeaponAttackController;

        public SoundController FlamethrowerWeaponAttackController => flamethrowerWeaponAttackController;
    }
}
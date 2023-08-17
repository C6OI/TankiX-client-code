using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class HammerShotAnimationComponent : AnimationTriggerComponent {
        [SerializeField] string shotTriggerName = "shot";

        [SerializeField] string isReloadingName = "isReloading";

        [SerializeField] string resetTriggerName = "reset";

        [SerializeField] string reloadingSpeedName = "reloadSpeedCoeff";

        [SerializeField] string cooldownSpeedName = "cooldownSpeedCoeff";

        [SerializeField] float idleTimeAfterCooldown = 0.5f;

        [SerializeField] AnimationClip reloadClip;

        [SerializeField] AnimationClip shotClip;

        int cooldownSpeedID;

        Animator hammerAnimator;

        int isReloadingID;

        int reloadingSpeedID;

        int resetTriggerID;

        int shotTriggerID;

        public AnimationClip ReloadClip {
            get => reloadClip;
            set => reloadClip = value;
        }

        public float RequiredReloadingTime { get; set; }

        public void Reset() {
            hammerAnimator.ResetTrigger(shotTriggerID);
            hammerAnimator.SetTrigger(resetTriggerID);
        }

        void OnChargeLastCartridge() => ProvideEvent<HammerChargeLastCartridgeEvent>();

        void OnBlowOff() => ProvideEvent<HammerBlowOffEvent>();

        void OnOffset() => ProvideEvent<HammerOffsetEvent>();

        void OnRoll() => ProvideEvent<HammerRollEvent>();

        void OnCartridgeClick() => ProvideEvent<HammerCartridgeClickEvent>();

        void OnMagazineShot() => ProvideEvent<HammerMagazineShotEvent>();

        void OnBounce() => ProvideEvent<HammerBounceEvent>();

        void OnCooldown() => ProvideEvent<HammerCooldownEvent>();

        public void InitHammerShotAnimation(Entity entity, Animator animator, float reloadingTimeSec,
            float shotCooldownLogicTime) {
            shotTriggerID = Animator.StringToHash(shotTriggerName);
            isReloadingID = Animator.StringToHash(isReloadingName);
            resetTriggerID = Animator.StringToHash(resetTriggerName);
            reloadingSpeedID = Animator.StringToHash(reloadingSpeedName);
            cooldownSpeedID = Animator.StringToHash(cooldownSpeedName);
            float length = reloadClip.length;
            float length2 = shotClip.length;
            float num = shotCooldownLogicTime - idleTimeAfterCooldown;
            float num3 = RequiredReloadingTime = reloadingTimeSec - num;
            float value = length / num3;
            float value2 = length2 / num;
            animator.SetFloat(reloadingSpeedID, value);
            animator.SetFloat(cooldownSpeedID, value2);
            hammerAnimator = animator;
            Entity = entity;
            enabled = true;
        }

        public void PlayShot() => Play(false);

        public void PlayShotAndReload() => Play(true);

        void Play(bool needReload) {
            hammerAnimator.ResetTrigger(resetTriggerID);
            hammerAnimator.SetTrigger(shotTriggerID);
            hammerAnimator.SetBool(isReloadingID, needReload);
        }
    }
}
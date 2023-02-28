using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class ContainerComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener {
        [SerializeField] Animator containerAnimator;

        [SerializeField] ParticleSystem[] dustParticles;

        [SerializeField] AudioSource openSound;

        [SerializeField] AudioSource closeSound;

        [SerializeField] string idleClosedAnimationName;

        [SerializeField] string closingAnimationName;

        Entity entity;

        public string assetGuid { get; set; }

        void AttachToEntityListener.AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void ShowOpenContainerAnimation() {
            PlayDustAnimators();
            openSound.Play();
            closeSound.Stop();
            containerAnimator.ResetTrigger("open");
            containerAnimator.SetTrigger("open");
        }

        public void ContainerOpend() {
            ScheduleEvent(new OpenContainerAnimationShownEvent(), entity);
        }

        public void PlayDustAnimators() {
            ParticleSystem[] array = dustParticles;

            foreach (ParticleSystem particleSystem in array) {
                particleSystem.Play();
            }
        }

        public void CloseContainer() {
            if (!InClosingState()) {
                openSound.Stop();
                closeSound.Play();
                containerAnimator.ResetTrigger("close");
                containerAnimator.SetTrigger("close");
            }
        }

        bool InClosingState() => containerAnimator.GetCurrentAnimatorStateInfo(0).IsName(idleClosedAnimationName) ||
                                 containerAnimator.GetCurrentAnimatorStateInfo(0).IsName(closingAnimationName);
    }
}
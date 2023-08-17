using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
    public class VisibilityPrerequisitesComponent : BehaviourComponent, ComponentLifecycle {
        readonly HashSet<string> hidePrerequisites = new();
        readonly HashSet<string> showPrerequisites = new();
        Animator animator;

        Animator Animator {
            get {
                if (animator == null) {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }

        bool ShouldBeHidden => showPrerequisites.Count == 0 || hidePrerequisites.Count > 0;

        public void AttachToEntity(Entity entity) {
            GetComponent<CanvasGroup>().alpha = 0f;
            RemoveAll();
        }

        public void DetachFromEntity(Entity entity) { }

        public void AddShowPrerequisite(string prerequisite, bool instant = false) {
            showPrerequisites.Add(prerequisite);
            UpdateVisibility(instant);
        }

        public void RemoveShowPrerequisite(string prerequisite, bool instant = false) {
            showPrerequisites.Remove(prerequisite);
            UpdateVisibility(instant);
        }

        public void AddHidePrerequisite(string prerequisite, bool instant = false) {
            hidePrerequisites.Add(prerequisite);
            UpdateVisibility(instant);
        }

        public void RemoveHidePrerequisite(string prerequisite, bool instant = false) {
            hidePrerequisites.Remove(prerequisite);
            UpdateVisibility(instant);
        }

        public void RemoveAll() {
            showPrerequisites.Clear();
            hidePrerequisites.Clear();
        }

        void UpdateVisibility(bool instant = false) {
            Animator.SetBool("NoAnimation", instant);
            Animator.SetBool("Visible", !ShouldBeHidden);
        }
    }
}
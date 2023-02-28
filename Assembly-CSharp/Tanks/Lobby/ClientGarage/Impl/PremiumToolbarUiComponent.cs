using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumToolbarUiComponent : BaseDialogComponent {
        public LocalizedField daysTextLocalizedField;

        public LocalizedField hoursTextLocalizedField;

        public TextMeshProUGUI activeText;

        public TextMeshProUGUI questText;

        public Animator animator;

        public bool visible;

        public void ActivatePremiumTasks() {
            questText.color = Color.white;
        }

        public void DeactivatePremiumTasks() {
            questText.color = Color.gray;
        }

        public void Toggle() {
            if (visible) {
                Hide();
            } else {
                Show();
            }
        }

        public void Hidden() {
            visible = false;
        }

        public void Visible() {
            visible = true;
        }

        public override void Hide() {
            if (visible) {
                animator.SetBool("visible", false);
            }
        }

        public override void Show(List<Animator> animators = null) {
            if (!visible) {
                animator.SetBool("visible", true);
            }
        }
    }
}
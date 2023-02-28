using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPModuleContainerHoverEnabler : HoverHandler {
        [SerializeField] Animator animator;

        protected override bool pointerIn {
            set {
                base.pointerIn = value;
                animator.SetBool("pointerIn", value);
            }
        }
    }
}
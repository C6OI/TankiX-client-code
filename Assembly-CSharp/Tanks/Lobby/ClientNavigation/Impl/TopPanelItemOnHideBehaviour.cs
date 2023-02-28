using System.Linq;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class TopPanelItemOnHideBehaviour : StateMachineBehaviour {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            bool active = animator.parameters.Any(p => p.name == "InactiveAfterHide") && !animator.GetBool("InactiveAfterHide");
            animator.gameObject.SetActive(active);
        }
    }
}
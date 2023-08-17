using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Lobby.ClientNavigation.API {
    public class ScreenVisibleBehaviour : ScreenBehaviour {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Entity entity = EngineService.EntityStub;

            EngineService.ExecuteInFlow(delegate(Engine e) {
                e.NewEvent<UnlockElementEvent>().Attach(entity).Schedule();
            });

            GetCanvasGroup(animator.gameObject).blocksRaycasts = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Entity entity = EngineService.EntityStub;

            EngineService.ExecuteInFlow(delegate(Engine e) {
                e.NewEvent<LockElementEvent>().Attach(entity).Schedule();
            });

            GetCanvasGroup(animator.gameObject).blocksRaycasts = false;
        }
    }
}
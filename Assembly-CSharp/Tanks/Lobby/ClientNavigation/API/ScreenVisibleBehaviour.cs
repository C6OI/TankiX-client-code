using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ScreenVisibleBehaviour : ScreenBehaviour {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Entity entityStub = EngineService.EntityStub;
            EngineService.Engine.NewEvent<UnlockElementEvent>().Attach(entityStub).Schedule();
            GetCanvasGroup(animator.gameObject).blocksRaycasts = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Entity entityStub = EngineService.EntityStub;
            EngineService.Engine.NewEvent<LockElementEvent>().Attach(entityStub).Schedule();
            GetCanvasGroup(animator.gameObject).blocksRaycasts = false;
        }
    }
}
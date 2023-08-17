using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Lobby.ClientControls.Impl {
    public class LoadGearSystem : ECSSystem {
        [OnEventFire]
        public void ShowLoadGear(ShowLoadGearEvent e, SingleNode<LoadGearComponent> loadGear) {
            LoadGearComponent component = loadGear.component;
            component.GearProgressBar.gameObject.SetActive(e.ShowProgress);
            component.GearProgressBar.ProgressValue = 0f;
            component.gameObject.SetActive(true);
        }

        [OnEventComplete]
        public void ShowLoadGear(ShowLoadGearEvent e, NotForegroundLoadGearNode loadGear,
            [JoinAll] Optional<ForegroundActiveLoadGearNode> foregroundActiveGear) {
            loadGear.Entity.AddComponent<ActiveGearComponent>();

            if (foregroundActiveGear.IsPresent() && foregroundActiveGear.Get().loadGear.gameObject.activeInHierarchy) {
                foregroundActiveGear.Get().loadGear.Animator.SetTrigger("hide");
            }
        }

        [OnEventFire]
        public void ShowLoadGear(ShowLoadGearEvent e, ForegroundLoadGearNode loadGear) {
            loadGear.Entity.AddComponent<ActiveGearComponent>();
            ScheduleEvent<ShowScreenForegroundEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadGear(HideLoadGearEvent e, SingleNode<LoadGearComponent> loadGear) {
            LoadGearComponent component = loadGear.component;

            if (component.gameObject.activeInHierarchy) {
                component.Animator.SetTrigger("hide");
            }
        }

        [OnEventComplete]
        public void HideLoadGear(HideLoadGearEvent e, NotForegroundActiveLoadGearNode loadGear,
            [JoinAll] Optional<ForegroundActiveLoadGearNode> foregroundActiveGear) {
            loadGear.Entity.RemoveComponent<ActiveGearComponent>();

            if (foregroundActiveGear.IsPresent() && !foregroundActiveGear.Get().loadGear.gameObject.activeInHierarchy) {
                foregroundActiveGear.Get().loadGear.Animator.SetTrigger("show");
            }
        }

        [OnEventFire]
        public void HideLoadGear(HideLoadGearEvent e, ForegroundActiveLoadGearNode loadGear) {
            loadGear.Entity.RemoveComponent<ActiveGearComponent>();
            ScheduleEvent<HideScreenForegroundEvent>(loadGear);
        }

        [OnEventFire]
        public void UpdateLoadGearProgress(UpdateLoadGearProgressEvent e, ActiveLoadGearNode loadGear) =>
            loadGear.loadGear.GearProgressBar.ProgressValue = e.Value;

        public class ForegroundLoadGearNode : LoadGearNode {
            public ScreenForegroundComponent screenForeground;
        }

        public class ForegroundActiveLoadGearNode : ActiveLoadGearNode {
            public ScreenForegroundComponent screenForeground;
        }

        [Not(typeof(ScreenForegroundComponent))]
        public class NotForegroundLoadGearNode : LoadGearNode { }

        [Not(typeof(ScreenForegroundComponent))]
        public class NotForegroundActiveLoadGearNode : ActiveLoadGearNode { }

        [Not(typeof(ActiveGearComponent))]
        public class LoadGearNode : Node {
            public LoadGearComponent loadGear;
        }

        public class ActiveLoadGearNode : Node {
            public ActiveGearComponent activeGear;
            public LoadGearComponent loadGear;
        }
    }
}
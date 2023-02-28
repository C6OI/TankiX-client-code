using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientControls.Impl {
    public class ScreenForegroundSystem : ECSSystem {
        [OnEventFire]
        public void SendShowScreenEvent(NodeAddedEvent e, SingleNode<ShowScreenForegroundComponent> screen, [JoinAll] ForegroundNode foreground) {
            ScheduleEvent(new ShowScreenForegroundEvent {
                Alpha = screen.component.Alpha
            }, foreground);
        }

        [OnEventFire]
        public void SendHideScreenEvent(NodeRemoveEvent e, SingleNode<ShowScreenForegroundComponent> screen, [JoinAll] ForegroundNode foreground) {
            ScheduleEvent<HideScreenForegroundEvent>(foreground);
        }

        [OnEventFire]
        public void ShowScreenForeground(ShowScreenForegroundEvent e, ForegroundNode foreground) {
            foreground.screenForeground.Count++;
            ScreenForegroundAnimationComponent screenForegroundAnimation = foreground.screenForegroundAnimation;
            screenForegroundAnimation.Animator.SetBool("visible", true);
            screenForegroundAnimation.Alpha = e.Alpha;
        }

        [OnEventFire]
        public void HideScreenForeground(HideScreenForegroundEvent e, ForegroundNode foreground) {
            if (foreground.screenForeground.Count > 0) {
                foreground.screenForeground.Count--;
            }

            if (foreground.screenForeground.Count == 0) {
                HideForeground(foreground);
            }
        }

        static void HideForeground(ForegroundNode foreground) {
            ScreenForegroundAnimationComponent screenForegroundAnimation = foreground.screenForegroundAnimation;
            screenForegroundAnimation.Animator.SetBool("visible", false);
        }

        [OnEventFire]
        public void ForceHideScreenForeground(ForceHideScreenForegroundEvent e, ForegroundNode foreground) {
            foreground.screenForeground.Count = 0;
            HideForeground(foreground);
        }

        public class ForegroundNode : Node {
            public ScreenForegroundComponent screenForeground;

            public ScreenForegroundAnimationComponent screenForegroundAnimation;
        }
    }
}
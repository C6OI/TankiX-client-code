using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class LinkNavigationSystem : ECSSystem {
        [OnEventFire]
        public void NavigateLink(NavigateLinkEvent e, Node node) {
            ParseLinkEvent parseLinkEvent = new();
            parseLinkEvent.Link = e.Link;
            ParseLinkEvent parseLinkEvent2 = parseLinkEvent;
            ScheduleEvent(parseLinkEvent2, node);

            if (!IsParsed(parseLinkEvent2)) {
                if (parseLinkEvent2.ParseMessage != null) {
                    Log.ErrorFormat("Link parse error: {0}, ParseMessage: {1}", e.Link, parseLinkEvent2.ParseMessage);
                } else {
                    Log.ErrorFormat("Link parse error: {0}", e.Link);
                }
            } else if (parseLinkEvent2.CustomNavigationEvent == null) {
                ShowScreenEvent showScreenEvent = new(parseLinkEvent2.ScreenType, AnimationDirection.LEFT);

                if (parseLinkEvent2.ScreenContext != null) {
                    showScreenEvent.SetContext(parseLinkEvent2.ScreenContext, parseLinkEvent2.ScreenContextAutoDelete);
                }

                ScheduleEvent(showScreenEvent, node);
            } else {
                parseLinkEvent2.CustomNavigationEvent.Schedule();
            }
        }

        [OnEventFire]
        public void NavigateLink(ButtonClickEvent e, SingleNode<LinkButtonComponent> button) {
            ScheduleEvent(new NavigateLinkEvent {
                Link = button.component.Link
            }, button);
        }

        bool IsParsed(ParseLinkEvent e) => e.CustomNavigationEvent != null || e.ScreenType != null;
    }
}
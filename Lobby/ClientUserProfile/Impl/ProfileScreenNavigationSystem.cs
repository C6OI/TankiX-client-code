using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ProfileScreenNavigationSystem : ECSSystem {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void SendShowProfileScreenEvent(ButtonClickEvent e, SingleNode<ProfileButtonComponent> node,
            [JoinAll] SelfUserNode selfUser) =>
            ScheduleEvent(new ShowProfileScreenEvent(selfUser.Entity.Id), EngineService.EntityStub);

        [OnEventFire]
        public void SendShowProfileScreenEvent(UserLabelAvatarClickEvent e, SingleNode<UserLabelComponent> userLabel,
            [JoinByUser] SomeUserNode someUser) =>
            ScheduleEvent(new ShowProfileScreenEvent(someUser.Entity.Id), EngineService.EntityStub);

        [OnEventFire]
        public void CreateContextAndShowProfileScreen(ShowProfileScreenEvent e, Node any, [JoinAll] SelfUserNode selfUser) {
            Entity entity = CreateEntity("ProfileScreenContext_userId : " + e.UserId);
            entity.AddComponent(new ProfileScreenContextComponent(e.UserId));
            ShowScreenDownEvent<ProfileScreenComponent> showScreenDownEvent = new();
            showScreenDownEvent.SetContext(entity, true);
            ScheduleEvent(showScreenDownEvent, EngineService.EntityStub);
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;
        }

        public class SomeUserNode : Node {
            public UserComponent user;
        }
    }
}
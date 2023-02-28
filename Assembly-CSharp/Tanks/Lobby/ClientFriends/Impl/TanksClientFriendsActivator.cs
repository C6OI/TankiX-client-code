using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientFriends.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class TanksClientFriendsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<FriendsScreenTemplate>();
            TemplateRegistry.Register<FriendSentNotificationTemplate>();
            EngineService.RegisterSystem(new FriendsKeeperSystem());
            EngineService.RegisterSystem(new FriendsBuilderSystem());
            EngineService.RegisterSystem(new FriendsListSystem());
            EngineService.RegisterSystem(new LobbyFriendsScreenSystem());
            EngineService.RegisterSystem(new UserLabelFriendsSystem());
            EngineService.RegisterSystem(new DisplayProfileScreenHeaderSystem());
            EngineService.RegisterSystem(new FriendsActionsOnProfileScreenSystem());
            EngineService.RegisterSystem(new FriendsBattleShowSystem());
            EngineService.RegisterSystem(new FriendsScreenSystem());
            EngineService.RegisterSystem(new InviteFriendsPopupSystem());
            EngineService.RegisterSystem(new FriendsBadgeSystem());
            EngineService.RegisterSystem(new FriendInteractionSystem());
            EngineService.RegisterSystem(new WaitingForInviteAnswerSystem());
        }

        protected override void Activate() { }
    }
}
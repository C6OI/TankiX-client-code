using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class TanksClientFriendsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new FriendsBattleShowSystem());
            EngineService.RegisterSystem(new FriendsScreenSystem());
        }

        protected override void Activate() { }
    }
}
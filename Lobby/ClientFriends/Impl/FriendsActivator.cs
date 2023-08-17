using Lobby.ClientFriends.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientFriends.Impl {
    public class FriendsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<FriendsScreenTemplate>();
            EngineService.RegisterSystem(new FriendsKeeperSystem());
            EngineService.RegisterSystem(new FriendsBuilderSystem());
            EngineService.RegisterSystem(new FriendsListSystem());
            EngineService.RegisterSystem(new FriendsScreenSystem());
            EngineService.RegisterSystem(new UserLabelFriendsSystem());
            EngineService.RegisterSystem(new DisplayProfileScreenHeaderSystem());
            EngineService.RegisterSystem(new FriendsActionsOnProfileScreenSystem());
        }
    }
}
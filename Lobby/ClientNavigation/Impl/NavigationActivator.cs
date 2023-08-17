using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientNavigation.Impl {
    public class NavigationActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<ScreenTemplate>();
            TemplateRegistry.Register<ErrorScreenTemplate>();
            EngineService.RegisterSystem(new TopPanelSystem());
            EngineService.RegisterSystem(new AttachToScreenSystem());
            EngineService.RegisterSystem(new NavigationSystem());
            EngineService.RegisterSystem(new ScreenLockSystem());
            EngineService.RegisterSystem(new ElementLockSystem());
            EngineService.RegisterSystem(new ErrorScreenSystem());
            EngineService.RegisterSystem(new ReportButtonSystem());
            EngineService.RegisterSystem(new ApplicationExitSystem());
            EngineService.RegisterSystem(new BackgroundSystem());
            EngineService.RegisterSystem(new LoadingErrorsSystem());
            EngineService.RegisterSystem(new SceneLoaderSystem());
            EngineService.RegisterSystem(new GoBackSoundEffectSystem());
            EngineService.RegisterSystem(new NavigationStatisticsSystem());
            EngineService.RegisterSystem(new DialogsSystem());
        }

        protected override void Activate() => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
            Entity entity = engine.CreateEntity("Navigation");
            entity.AddComponent<HistoryComponent>();
            entity.AddComponent<CurrentScreenComponent>();
            entity.AddComponent<ScreensRegistryComponent>();
        });
    }
}
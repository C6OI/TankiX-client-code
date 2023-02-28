using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class NavigationActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<ScreenTemplate>();
            EngineService.RegisterSystem(new TopPanelSystem());
            EngineService.RegisterSystem(new AttachToScreenSystem());
            EngineService.RegisterSystem(new NavigationSystem());
            EngineService.RegisterSystem(new ScreenLockSystem());
            EngineService.RegisterSystem(new ElementLockSystem());
            EngineService.RegisterSystem(new BackgroundSystem());
            EngineService.RegisterSystem(new LoadingErrorsSystem());
            EngineService.RegisterSystem(new SceneLoaderSystem());
            EngineService.RegisterSystem(new GoBackSoundEffectSystem());
            EngineService.RegisterSystem(new ApplicationExitSystem());
            EngineService.RegisterSystem(new NavigationStatisticsSystem());
            EngineService.RegisterSystem(new DialogsSystem());
            EngineService.RegisterSystem(new LinkNavigationSystem());
        }

        protected override void Activate() {
            Entity entity = EngineService.Engine.CreateEntity("Navigation");
            entity.AddComponent<HistoryComponent>();
            entity.AddComponent<CurrentScreenComponent>();
            entity.AddComponent<ScreensRegistryComponent>();
        }
    }
}
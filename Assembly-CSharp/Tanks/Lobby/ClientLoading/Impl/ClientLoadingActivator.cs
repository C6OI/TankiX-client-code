using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientLoading.API;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class ClientLoadingActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new AssetBundleLoadingProgressBarSystem());
            EngineService.RegisterSystem(new AssetBundleLoadingSystem());
            EngineService.RegisterSystem(new PreloadAllResourcesScreenSystem());
            EngineService.RegisterSystem(new BattleLoadScreenSystem());
            EngineService.RegisterSystem(new OutputLogSystem());
            EngineService.RegisterSystem(new IntroCinematicSystem());
            TemplateRegistry.Register<PreloadAllResourcesScreenTemplate>();
            TemplateRegistry.Register<LobbyLoadScreenTemplate>();
            TemplateRegistry.Register<WarmupResourcesTemplate>();
        }
    }
}
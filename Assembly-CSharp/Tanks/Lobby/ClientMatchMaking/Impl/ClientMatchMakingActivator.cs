using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientMatchMaking.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class ClientMatchMakingActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<MatchMakingTemplate>();
            TemplateRegistry.Register<MatchMakingModeTemplate>();
            TemplateRegistry.Register<MatchMakingLobbyTemplate>();
            EngineService.RegisterSystem(new PlayButtonViewSystem());
            EngineService.RegisterSystem(new PlayButtonStateSystem());
            EngineService.RegisterSystem(new PlayButtonClickSystem());
            EngineService.RegisterSystem(new MatchMakingEntranceSystem());
            EngineService.RegisterSystem(new MatchMakingLobbySystem());
            EngineService.RegisterSystem(new UserEquipmentSystem());
            EngineService.RegisterSystem(new MatchMakingDefaultModeSystem());
            EngineService.RegisterSystem(new GameModeItemGUISystem());
            EngineService.RegisterSystem(new MatchMakingLobbyGUISystem());
            EngineService.RegisterSystem(new PlayScreenSystem());
            EngineService.RegisterSystem(new CustomBattlesScreenSystem());
            EngineService.RegisterSystem(new PlayAgainSystem());
            EngineService.RegisterSystem(new MatchMakingMapPreloadingSystem());
        }

        protected override void Activate() { }
    }
}
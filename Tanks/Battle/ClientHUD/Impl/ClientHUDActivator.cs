using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientHUD.API;
using Tanks.Battle.Hud.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ClientHUDActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterSystems();
            TemplateRegistry.Register<BattleChatHUDTemplate>();
            TemplateRegistry.Register<CombatLogMessagesTemplate>();
            TemplateRegistry.Register<HUDWorldSpaceCanvasTemplate>();
            TemplateRegistry.Register<DMBattleScreenTemplate>();
            TemplateRegistry.Register<PauseServiceMessageTemplate>();
            TemplateRegistry.Register<SelfDestructionServiceMessageTemplate>();
            TemplateRegistry.Register<UpsideDownServiceMessageTemplate>();
            TemplateRegistry.Register<AutokickServiceMessageTemplate>();
            TemplateRegistry.Register<UserNotificatorHUDTemplate>();
            TemplateRegistry.Register<UserNotificatorRankNamesTemplate>();
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new HUDScoreTableSystem());
            EngineService.RegisterSystem(new HUDBuilderSystem());
            EngineService.RegisterSystem(new NameplateBuilderSystem());
            EngineService.RegisterSystem(new NameplatePositioningSystem());
            EngineService.RegisterSystem(new NameplateOpacitySystem());
            EngineService.RegisterSystem(new ItemButtonBuilderSystem());
            EngineService.RegisterSystem(new ItemButtonQuantitySystem());
            EngineService.RegisterSystem(new ItemButtonCooldownStateSystem());
            EngineService.RegisterSystem(new ItemButtonEnabledStateSystem());
            EngineService.RegisterSystem(new ItemButtonDisabledStateSystem());
            EngineService.RegisterSystem(new ItemButtonActivatedStateSystem());
            EngineService.RegisterSystem(new ItemButtonVisibilitySystem());
            EngineService.RegisterSystem(new ItemButtonIconBlinkSystem());
            EngineService.RegisterSystem(new ButtonSupplyJoinSystem());
            EngineService.RegisterSystem(new HealthBarSystem());
            EngineService.RegisterSystem(new WeaponEnergyBarSystem());
            EngineService.RegisterSystem(new RailgunEnergyBarSystem());
            EngineService.RegisterSystem(new VulcanEnergyBarSystem());
            EngineService.RegisterSystem(new HammerEnergyBarSystem());
            EngineService.RegisterSystem(new WeaponBarVisibilitySystem());
            EngineService.RegisterSystem(new TabSystem());
            EngineService.RegisterSystem(new BattleScreenSystem());
            EngineService.RegisterSystem(new HUDTimeProgressBarSystem());
            EngineService.RegisterSystem(new DMScoreProgressBarSystem());
            EngineService.RegisterSystem(new IndicationFllagRemotenessSystem());
            EngineService.RegisterSystem(new UIElementsVisibilityControllerSystem());
            EngineService.RegisterSystem(new HUDFundSystem());
            EngineService.RegisterSystem(new CombatEventLogSystem());
            EngineService.RegisterSystem(new DMCombatEventLogSystem());
            EngineService.RegisterSystem(new TDMCombatEventLogSystem());
            EngineService.RegisterSystem(new CTFCombatEventLogSystem());
            EngineService.RegisterSystem(new ServiceMessageSystem());
            EngineService.RegisterSystem(new ServiceMessagesInteroperabilitySystem());
            EngineService.RegisterSystem(new SelfDestructionHUDSystem());
            EngineService.RegisterSystem(new PauseAndIdleKickHUDSystem());
            EngineService.RegisterSystem(new UpsideDownHUDSystem());
            EngineService.RegisterSystem(new SpectatorHUDModesSystem());
            EngineService.RegisterSystem(new SpectatorHUDSystem());
            EngineService.RegisterSystem(new RoundSpecificVisibilityControllerSystem());
            EngineService.RegisterSystem(new BattleChatSystem());
            EngineService.RegisterSystem(new BattleHUDStateControllerSystem());
            EngineService.RegisterSystem(new BattleInputContextSystem());
            EngineService.RegisterSystem(new BattleSpectatorInputContextSystem());
            EngineService.RegisterSystem(new BattleChatChannelSwitchSystem());
            EngineService.RegisterSystem(new ReceiveBattleMessageSystem());
            EngineService.RegisterSystem(new SendBattleMessageSystem());
            EngineService.RegisterSystem(new ResizeBattleChatScrollViewSystem());
            EngineService.RegisterSystem(new BattleChatVisibilitySystem());
            EngineService.RegisterSystem(new BattleChatInputSystem());
            EngineService.RegisterSystem(new UserNotificationHUDSystem());
        }
    }
}
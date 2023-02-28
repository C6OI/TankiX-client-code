using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ClientHUDActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

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
            TemplateRegistry.RegisterPart<EffectHUDTemplate>();
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new HUDScoreTableSystem());
            EngineService.RegisterSystem(new TeamBattleScoreTableSystem());
            EngineService.RegisterSystem(new HUDBuilderSystem());
            EngineService.RegisterSystem(new NameplateBuilderSystem());
            EngineService.RegisterSystem(new NameplatePositioningSystem());
            EngineService.RegisterSystem(new NameplateOpacitySystem());
            EngineService.RegisterSystem(new ItemBundleLimitBundleEffectsSystem());
            EngineService.RegisterSystem(new ItemButtonAmmunitionSystem());
            EngineService.RegisterSystem(new ItemButtonCooldownStateSystem());
            EngineService.RegisterSystem(new ItemButtonEnabledStateSystem());
            EngineService.RegisterSystem(new ItemButtonPassiveStateSystem());
            EngineService.RegisterSystem(new InventorySlotActivationSystem());
            EngineService.RegisterSystem(new TabSystem());
            EngineService.RegisterSystem(new BattleScreenSystem());
            EngineService.RegisterSystem(new RoundFinishSystem());
            EngineService.RegisterSystem(new UIElementsVisibilityControllerSystem());
            EngineService.RegisterSystem(new CombatEventLogSystem());
            EngineService.RegisterSystem(new DMCombatEventLogSystem());
            EngineService.RegisterSystem(new TDMCombatEventLogSystem());
            EngineService.RegisterSystem(new CTFCombatEventLogSystem());
            EngineService.RegisterSystem(new VisualScoreSystem());
            EngineService.RegisterSystem(new ServiceMessageSystem());
            EngineService.RegisterSystem(new ServiceMessagesInteroperabilitySystem());
            EngineService.RegisterSystem(new SelfDestructionHUDSystem());
            EngineService.RegisterSystem(new PauseAndIdleKickHUDSystem());
            EngineService.RegisterSystem(new UpsideDownHUDSystem());
            EngineService.RegisterSystem(new SpectatorHUDModesSystem());
            EngineService.RegisterSystem(new RoundSpecificVisibilityControllerSystem());
            EngineService.RegisterSystem(new BattleChatSystem());
            EngineService.RegisterSystem(new BattleHUDStateControllerSystem());
            EngineService.RegisterSystem(new BattleInputContextSystem());
            EngineService.RegisterSystem(new BattleSpectatorInputContextSystem());
            EngineService.RegisterSystem(new BattleChatChannelSwitchSystem());
            EngineService.RegisterSystem(new ReceiveBattleMessageSystem());
            EngineService.RegisterSystem(new ResizeBattleChatScrollViewSystem());
            EngineService.RegisterSystem(new BattleChatVisibilitySystem());
            EngineService.RegisterSystem(new BattleChatInputSystem());
            EngineService.RegisterSystem(new UserNotificationHUDSystem());
            EngineService.RegisterSystem(new MainHUDSystem());
            EngineService.RegisterSystem(new InventoryHUDSystem());
            EngineService.RegisterSystem(new HealthBarSystem());
            EngineService.RegisterSystem(new EffectsHUDSystem());
            EngineService.RegisterSystem(new EffectHUDCooldownSpeedSystem());
            EngineService.RegisterSystem(new LockSlotByEMPSystem());
            EngineService.RegisterSystem(new EnergyInjectionEffectHUDSystem());
            EngineService.RegisterSystem(new RageHUDEffectSystem());
            EngineService.RegisterSystem(new DMScoreHUDSystem());
            EngineService.RegisterSystem(new TeamScoreHUDSystem());
            EngineService.RegisterSystem(new CTFFlagsHUDSystem());
            EngineService.RegisterSystem(new CTFPointersSystem());
            EngineService.RegisterSystem(new WarmingUpTimerSystem());
            EngineService.RegisterSystem(new WarmingUpTimerNotificationsSystem());
            EngineService.RegisterSystem(new DiscreteEnergyBarSystem());
            EngineService.RegisterSystem(new HammerEnergyBarSystem());
            EngineService.RegisterSystem(new TwinsEnergyBarSystem());
            EngineService.RegisterSystem(new RailgunEnergyBarSystem());
            EngineService.RegisterSystem(new ShaftEnergyBarSystem());
            EngineService.RegisterSystem(new StreamEnergyBarSystem());
            EngineService.RegisterSystem(new VulcanEnergyBarSystem());
            EngineService.RegisterSystem(new MultikillSystem());
            EngineService.RegisterSystem(new DisbalanceInfoSystem());
            EngineService.RegisterSystem(new TutorialHUDSystem());
            EngineService.RegisterSystem(new DamageInfoSystem());
            EngineService.RegisterSystem(new SelfTargetHitFeedbackHUDSystem());
        }
    }
}
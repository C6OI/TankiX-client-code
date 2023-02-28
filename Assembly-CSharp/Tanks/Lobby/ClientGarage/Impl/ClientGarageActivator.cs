using Lobby.ClientGarage.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.API.Energy;
using Tanks.Lobby.ClientGarage.Impl.Tutorial;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ClientGarageActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegistersTemplates();
            RegisterSystems();
            RegisterPayableSystemsAndTemplates();
            RegisterPremiumSystemsAndTemplates();
        }

        protected override void Activate() {
            Engine engine = EngineService.Engine;
            Entity entity = engine.CreateEntity<GarageTemplate>("garage");
            UpgradablePropertiesUtils.MAX_LEVEL = entity.GetComponent<ItemUpgradeExperiencesConfigComponent>().LevelsExperiences.Length + 1;
            engine.CreateEntity<SlotConfigTemplate>("garage/module/slot");
            engine.CreateEntity<ModuleTierConfigTemplate>("garage/module/tier");
            engine.CreateEntity<TutorialsConfigurationTemplate>("garage/tutorial/config");
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new TutorialBuildSystem());
            EngineService.RegisterSystem(new TutorialShowSystem());
            EngineService.RegisterSystem(new TutorialSkipSystem());
            EngineService.RegisterSystem(new ProfileSummarySectionUISystem());
            EngineService.RegisterSystem(new ProfileStatsSectionUISystem());
            EngineService.RegisterSystem(new ProfileAccountSectionUISystem());
            EngineService.RegisterSystem(new LogoutSystem());
            EngineService.RegisterSystem(new UserEmailShowSystem());
            EngineService.RegisterSystem(new LeagueRewardScreenSystem());
            EngineService.RegisterSystem(new ModulesBadgesSystem());
            EngineService.RegisterSystem(new LoginRewardSystem());
            EngineService.RegisterSystem(new ModuleSystem());
            EngineService.RegisterSystem(new MountItemSystem());
            EngineService.RegisterSystem(new GarageItemsScreenSystem());
            EngineService.RegisterSystem(new ItemsListScreenSystem());
            EngineService.RegisterSystem(new GarageListItemContentSystem());
            EngineService.RegisterSystem(new GarageNavigationSystem());
            EngineService.RegisterSystem(new QuickRegistrartionNavigationSystem());
            EngineService.RegisterSystem(new DisplayMountButtonSystem());
            EngineService.RegisterSystem(new DisplayBuyButtonSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionCheckSystem());
            EngineService.RegisterSystem(new DisplayUserItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new DisplayMarketItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new GarageItemsCategoryScreenSystem());
            EngineService.RegisterSystem(new PriceLabelSystem());
            EngineService.RegisterSystem(new XPriceLabelSystem());
            EngineService.RegisterSystem(new CreateBuyItemPacksButtonsSystem());
            EngineService.RegisterSystem(new PresetSystem(EngineService.SystemRegistry));
            EngineService.RegisterSystem(new HiddenInGarageItemSystem());
            EngineService.RegisterSystem(new UserItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new BlockKeyboardForListOnConfirmButtonSystem());
            EngineService.RegisterSystem(new GarageSaleSystem());
            EngineService.RegisterSystem(new PromoCodeSystem());
            EngineService.RegisterSystem(new ContainersScreenSystem());
            EngineService.RegisterSystem(new ContainerContentScreenSystem());
            EngineService.RegisterSystem(new DisplayOpenContainerButtonSystem());
            EngineService.RegisterSystem(new DisplayContainerContentButtonSystem());
            EngineService.RegisterSystem(new GarageItemOnlyInContainerUISystem());
            EngineService.RegisterSystem(new BuyContainersSystem());
            EngineService.RegisterSystem(new BuyEnergySystem());
            EngineService.RegisterSystem(new MarketInfoSystem());
            EngineService.RegisterSystem(new MarketItemPriceSystem());
            RegisterPropertiesSystems();
            EngineService.RegisterSystem(new SlotsSystem());
            EngineService.RegisterSystem(new TankPartItemStatBuilderSystem());
            EngineService.RegisterSystem(new ModulesListSystem());
            EngineService.RegisterSystem(new ModulesScreenSystem());
            EngineService.RegisterSystem(new NewModulesScreenSystem());
            EngineService.RegisterSystem(new ModulesTutorialSystem());
            EngineService.RegisterSystem(new ModulesButtonsSystem());
            EngineService.RegisterSystem(new PresetUISystem());
            EngineService.RegisterSystem(new CustomizationUISystem());
            EngineService.RegisterSystem(new MainScreenSystem());
            EngineService.RegisterSystem(new ChangeScreenLogSystem());
            EngineService.RegisterSystem(new GoldBonusCounterSystem());
            EngineService.RegisterSystem(new GaragePriceSystem());
            GarageItemsRegistrySystem garageItemsRegistrySystem = new();
            ServiceRegistry.Current.RegisterService((GarageItemsRegistry)garageItemsRegistrySystem);
            EngineService.RegisterSystem(garageItemsRegistrySystem);
            EngineService.RegisterSystem(new ArmsRaceUISystem());
            EngineService.RegisterSystem(new ItemUnlockUISystem());
            EngineService.RegisterSystem(new WelcomeScreenSystem());
            EngineService.RegisterSystem(new DailyBonusScreenSystem());
            EngineService.RegisterSystem(new ExitDialogSystem());
            EngineService.RegisterSystem(new ReleaseGiftsScreenSystem());
            EngineService.RegisterSystem(new PremiumUiShopSystem());
            EngineService.RegisterSystem(new PremiumLearnMoreSystem());
            EngineService.RegisterSystem(new PremiumMoneyBonusSystem());
            EngineService.RegisterSystem(new GoldBoxesUiShopSystem());
            EngineService.RegisterSystem(new GoldBoxesLearnMoreSystem());
            RegisterAvatarSystems();
        }

        void RegisterPropertiesSystems() {
            EngineService.RegisterSystem(new ContainerSystem());
            EngineService.RegisterSystem(new OpenContainerSystem());
        }

        void RegisterAvatarSystems() {
            EngineService.RegisterSystem(new AvatarMenuSystem());
            EngineService.RegisterSystem(new AvatarScreenSystem());
        }

        void RegistersTemplates() {
            TemplateRegistry.Register<GarageItemTemplate>();
            TemplateRegistry.Register<ItemImagedTemplate>();
            TemplateRegistry.Register<CardTierTemplate>();
            TemplateRegistry.Register<GarageItemImagedTemplate>();
            TemplateRegistry.Register<MarketItemTemplate>();
            TemplateRegistry.Register<UserItemTemplate>();
            TemplateRegistry.Register<UpgradableUserItemTemplate>();
            TemplateRegistry.Register<GarageTemplate>();
            TemplateRegistry.Register<MountItemButtonTemplate>();
            TemplateRegistry.Register<GarageItemsTemplate>();
            TemplateRegistry.Register<DurationItemTemplate>();
            RegisterPresetTemplates();
            RegisterWeaponTemplates();
            RegisterTankTemplates();
            RegisterPaintTemplates();
            RegisterGraffitiTemplates();
            RegisterAvatarTemplates();
            RegisterButtonWithPriceTemplates();
            RegisterShellTemplates();
            RegisterSkinTemplates();
            RegisterContainerTemplates();
            RegisterCrystalTemplates();
            RegisterModulesTemplates();
            RegisterEnergyTemplates();
            RegisterTankTemplates();
            RegisterTutorialTemplates();
            RegisterDetailTemplates();
            RegisterGoldBonusTemplates();
            TemplateRegistry.Register<EnergyDailyBonusTemplate>();
            TemplateRegistry.Register<QuestDailyBonusTemplate>();
            TemplateRegistry.Register<MoneyDailyBonusTemplate>();
        }

        void RegisterPresetTemplates() {
            TemplateRegistry.Register<PresetUserItemTemplate>();
            TemplateRegistry.Register<PresetMarketItemTemplate>();
        }

        void RegisterWeaponTemplates() {
            TemplateRegistry.Register<WeaponItemTemplate>();
            TemplateRegistry.Register<WeaponMarketItemTemplate>();
            TemplateRegistry.Register<HammerMarketItemTemplate>();
            TemplateRegistry.Register<HammerUserItemTemplate>();
            TemplateRegistry.Register<RailgunMarketItemTemplate>();
            TemplateRegistry.Register<RailgunUserItemTemplate>();
            TemplateRegistry.Register<RicochetMarketItemTemplate>();
            TemplateRegistry.Register<RicochetUserItemTemplate>();
            TemplateRegistry.Register<ShaftMarketItemTemplate>();
            TemplateRegistry.Register<ShaftUserItemTemplate>();
            TemplateRegistry.Register<SmokyMarketItemTemplate>();
            TemplateRegistry.Register<SmokyUserItemTemplate>();
            TemplateRegistry.Register<ThunderMarketItemTemplate>();
            TemplateRegistry.Register<ThunderUserItemTemplate>();
            TemplateRegistry.Register<TwinsMarketItemTemplate>();
            TemplateRegistry.Register<TwinsUserItemTemplate>();
            TemplateRegistry.Register<FlamethrowerMarketItemTemplate>();
            TemplateRegistry.Register<FlamethrowerUserItemTemplate>();
            TemplateRegistry.Register<FreezeMarketItemTemplate>();
            TemplateRegistry.Register<FreezeUserItemTemplate>();
            TemplateRegistry.Register<VulcanMarketItemTemplate>();
            TemplateRegistry.Register<VulcanUserItemTemplate>();
            TemplateRegistry.Register<IsisMarketItemTemplate>();
            TemplateRegistry.Register<IsisUserItemTemplate>();
        }

        void RegisterTankTemplates() {
            TemplateRegistry.Register<TankUserItemTemplate>();
            TemplateRegistry.Register<TankMarketItemTemplate>();
        }

        void RegisterShellTemplates() {
            TemplateRegistry.Register<ShellMarketItemTemplate>();
            TemplateRegistry.Register<ShellUserItemTemplate>();
        }

        void RegisterSkinTemplates() {
            TemplateRegistry.Register<HullSkinMarketItemTemplate>();
            TemplateRegistry.Register<HullSkinUserItemTemplate>();
            TemplateRegistry.Register<WeaponSkinMarketItemTemplate>();
            TemplateRegistry.Register<WeaponSkinUserItemTemplate>();
        }

        void RegisterPaintTemplates() {
            TemplateRegistry.Register<TankPaintMarketItemTemplate>();
            TemplateRegistry.Register<TankPaintUserItemTemplate>();
            TemplateRegistry.Register<WeaponPaintMarketItemTemplate>();
            TemplateRegistry.Register<WeaponPaintUserItemTemplate>();
        }

        void RegisterGraffitiTemplates() {
            TemplateRegistry.Register<GraffitiMarketItemTemplate>();
            TemplateRegistry.Register<ChildGraffitiMarketItemTemplate>();
            TemplateRegistry.Register<GraffitiUserItemTemplate>();
        }

        void RegisterAvatarTemplates() {
            TemplateRegistry.Register<AvatarMarketItemTemplate>();
            TemplateRegistry.Register<AvatarUserItemTemplate>();
        }

        void RegisterButtonWithPriceTemplates() {
            TemplateRegistry.Register<BuyButtonWithPriceTemplate>();
        }

        void RegisterPayableSystemsAndTemplates() {
            TemplateRegistry.Register<ChangeUIDTemplate>();
            EngineService.RegisterSystem(new ChangeUIDSystem());
        }

        void RegisterContainerTemplates() {
            TemplateRegistry.Register<ContainerMarketItemTemplate>();
            TemplateRegistry.Register<ContainerPackPriceMarketItemTemplate>();
            TemplateRegistry.Register<ContainerUserItemTemplate>();
            TemplateRegistry.Register<GameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<DonutChestMarketItemTemplate>();
            TemplateRegistry.Register<GameplayChestUserItemTemplate>();
            TemplateRegistry.Register<SimpleChestUserItemTemplate>();
            TemplateRegistry.Register<CheatGameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<CheatGameplayChestUserItemTemplate>();
            TemplateRegistry.Register<TutorialGameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<TutorialGameplayChestUserItemTemplate>();
        }

        void RegisterCrystalTemplates() {
            TemplateRegistry.Register<CrystalMarketItemTemplate>();
            TemplateRegistry.Register<CrystalUserItemTemplate>();
            TemplateRegistry.Register<XCrystalMarketItemTemplate>();
            TemplateRegistry.Register<XCrystalUserItemTemplate>();
        }

        void RegisterGoldBonusTemplates() {
            TemplateRegistry.Register<GoldBonusMarketItemTemplate>();
            TemplateRegistry.Register<GoldBonusUserItemTemplate>();
        }

        void RegisterEnergyTemplates() {
            TemplateRegistry.Register<EnergyMarketItemTemplate>();
            TemplateRegistry.Register<EnergyUserItemTemplate>();
            TemplateRegistry.Register<QuantumMarketItemTemplate>();
        }

        void RegisterDetailTemplates() {
            TemplateRegistry.Register<DetailItemTemplate>();
            TemplateRegistry.Register<DetailMarketItemTemplate>();
            TemplateRegistry.Register<DetailUserItemTemplate>();
        }

        void RegisterModulesTemplates() {
            TemplateRegistry.Register<SlotMarketItemTemplate>();
            TemplateRegistry.Register<SlotUserItemTemplate>();
            TemplateRegistry.Register<SlotConfigTemplate>();
            TemplateRegistry.Register<ModuleTierConfigTemplate>();
            TemplateRegistry.Register<ModuleMarketItemTemplate>();
            TemplateRegistry.Register<ModuleUserItemTemplate>();
            TemplateRegistry.Register<PassiveModuleUserItemTemplate>();
            TemplateRegistry.Register<TriggerModuleUserItemTemplate>();
            TemplateRegistry.Register<GoldBonusModuleUserItemTemplate>();
            TemplateRegistry.Register<GarageModulesScreenTemplate>();
            TemplateRegistry.Register<ModuleCardItemTemplate>();
            TemplateRegistry.Register<ModuleCardMarketItemTemplate>();
            TemplateRegistry.Register<ModuleCardUserItemTemplate>();
            TemplateRegistry.Register<ModuleUpgradeInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEffectWithDurationTemplate>();
            TemplateRegistry.Register<ModuleUpgradeArmorEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeTurbospeedEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeTempblockEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeDamageEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeHealingEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeAcceleratedGearsEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEngineerEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeLifestealEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeBackhitModificatorEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEmergencyProtectionEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeRageEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeCommonMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeIcetrapEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeSpiderMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeDroneEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEnergyInjectionEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeAdrenalineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEMPEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeForcefieldEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeGoldBonusActionInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeJumpEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeExternalImpactEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeFireRingEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeExplosiveMassEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeSapperEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeKamikadzeEffectInfoTemplate>();
        }

        void RegisterTutorialTemplates() {
            TemplateRegistry.Register<TutorialsConfigurationTemplate>();
            TemplateRegistry.Register<TutorialDataTemplate>();
            TemplateRegistry.Register<TutorialStepDataTemplate>();
            TemplateRegistry.Register<TutorialHighlightTankDataTemplate>();
            TemplateRegistry.Register<TutorialSelectItemDataTemplate>();
            TemplateRegistry.Register<TutorialRewardDataTemplate>();
        }

        void RegisterPremiumSystemsAndTemplates() {
            TemplateRegistry.Register<PremiumBoostMarketItemTemplate>();
            TemplateRegistry.Register<PremiumBoostUserItemTemplate>();
            TemplateRegistry.Register<PremiumQuestMarketItemTemplate>();
            TemplateRegistry.Register<PremiumQuestUserItemTemplate>();
            EngineService.RegisterSystem(new PremiumItemSystem());
        }
    }
}
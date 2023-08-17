using Lobby.ClientGarage.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ClientGarageActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegistersTemplates();
            RegisterSystems();
            RegisterPayableSystemsAndTemplates();
        }

        protected override void Activate() => EngineService.ExecuteInFlow(delegate(Engine e) {
            e.CreateEntity<TankTemperatureConfigTemplate>("tank/temperature_config");
        });

        void RegisterSystems() {
            EngineService.RegisterSystem(new BuyMarketItemSystem());
            EngineService.RegisterSystem(new MountItemSystem());
            EngineService.RegisterSystem(new GarageItemsScreenSystem());
            EngineService.RegisterSystem(new ItemsListScreenSystem());
            EngineService.RegisterSystem(new DisplayMountButtonSystem());
            EngineService.RegisterSystem(new DisplayBuyButtonSystem());
            EngineService.RegisterSystem(new DisplayPropertyButtonSystem());
            EngineService.RegisterSystem(new DisplayShellsButtonSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionCheckSystem());
            EngineService.RegisterSystem(new DisplayUserItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new DisplayMarketItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new DisplaySkinsButtonSystem());
            EngineService.RegisterSystem(new GarageItemsCategoryScreenSystem());
            EngineService.RegisterSystem(new GarageSuppliesScreenSystem());
            EngineService.RegisterSystem(new BuySuppliesSystem());
            EngineService.RegisterSystem(new PropertiesScreenSystem());
            EngineService.RegisterSystem(new DisplaySupplyPropertiesSystem());
            EngineService.RegisterSystem(new UpgradePropertiesScreenSystem());
            EngineService.RegisterSystem(new AttributeTextSystem());
            EngineService.RegisterSystem(new PriceLabelSystem());
            EngineService.RegisterSystem(new XPriceLabelSystem());
            EngineService.RegisterSystem(new UserItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new BlockKeyboardForListOnConfirmButtonSystem());
            RegisterPropertiesSystems();
        }

        void RegisterPropertiesSystems() {
            EngineService.RegisterSystem(new GarageItemPropertySystem());
            EngineService.RegisterSystem(new AccelerationPropertySystem());
            EngineService.RegisterSystem(new HealthPropertySystem());
            EngineService.RegisterSystem(new SpeedPropertySystem());
            EngineService.RegisterSystem(new TurnSpeedPropertySystem());
            EngineService.RegisterSystem(new WeightPropertySystem());
            EngineService.RegisterSystem(new BulletSpeedPropertySystem());
            EngineService.RegisterSystem(new CriticalDamagePropertySystem());
            EngineService.RegisterSystem(new CriticalProbabilityPropertySystem());
            EngineService.RegisterSystem(new DamagePerSecondPropertySystem());
            EngineService.RegisterSystem(new DamageWeakeningByTargetPropertySystem());
            EngineService.RegisterSystem(new EnergyChargeSpeedPropertySystem());
            EngineService.RegisterSystem(new EnergyRechargeSpeedPropertySystem());
            EngineService.RegisterSystem(new HammerDamagePropertySystem());
            EngineService.RegisterSystem(new MagazineSizePropertySystem());
            EngineService.RegisterSystem(new ReloadMagazineTimePropertySystem());
            EngineService.RegisterSystem(new ImpactPropertySystem());
            EngineService.RegisterSystem(new HealingPropertySystem());
            EngineService.RegisterSystem(new SelfHealingPropertySystem());
            EngineService.RegisterSystem(new RangeDamagePropertySystem());
            EngineService.RegisterSystem(new DamageDistancePropertySystem());
            EngineService.RegisterSystem(new MinDamageDistancePropertySystem());
            EngineService.RegisterSystem(new ReloadTimePropertySystem());
            EngineService.RegisterSystem(new AimingDamagePropertySystem());
            EngineService.RegisterSystem(new TurretTurnSpeedPropertySystem());
            EngineService.RegisterSystem(new SpinUpTimePropertySystem());
            EngineService.RegisterSystem(new TemperatureHittingTimePropertySystem());
            EngineService.RegisterSystem(new FireDamagePropertySystem());
            EngineService.RegisterSystem(new RicochetShotsCountPropertySystem());
            EngineService.RegisterSystem(new SupplyPropertyBuilderSystem());
            EngineService.RegisterSystem(new ArmorSupplyPropertySystem());
            EngineService.RegisterSystem(new CooldownSupplyPropertySystem());
            EngineService.RegisterSystem(new DamageSupplyPropertySystem());
            EngineService.RegisterSystem(new DurationSupplyPropertySystem());
            EngineService.RegisterSystem(new MineSupplyPropertySystem());
            EngineService.RegisterSystem(new RepairSupplyPropertySystem());
            EngineService.RegisterSystem(new SpeedSupplyPropertySystem());
        }

        void RegistersTemplates() {
            TemplateRegistry.Register<TankTemperatureConfigTemplate>();
            TemplateRegistry.Register<GarageItemTemplate>();
            TemplateRegistry.Register<ItemImagedTemplate>();
            TemplateRegistry.Register<GarageItemImagedTemplate>();
            TemplateRegistry.Register<MarketItemTemplate>();
            TemplateRegistry.Register<MarketItemTemplate>();
            TemplateRegistry.Register<UserItemTemplate>();
            TemplateRegistry.Register<UpgradableUserItemTemplate>();
            TemplateRegistry.Register<GarageTemplate>();
            TemplateRegistry.Register<MountItemButtonTemplate>();
            TemplateRegistry.Register<PropertyItemButtonTemplate>();
            TemplateRegistry.Register<ShellsButtonTemplate>();
            TemplateRegistry.Register<GarageItemsCategoryScreenTemplate>();
            TemplateRegistry.Register<GarageItemPropertyTemplate>();
            TemplateRegistry.Register<GarageItemsTemplate>();
            TemplateRegistry.Register<ItemPropertyTemplate>();
            RegisterWeaponTemplates();
            RegisterTankTemplates();
            RegisterPaintTemplates();
            RegisterGraffitiTemplates();
            RegisterSupplyTemplates();
            RegisterShellTemplates();
            RegisterSkinTemplates();
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
            TemplateRegistry.Register<PaintMarketItemTemplate>();
            TemplateRegistry.Register<PaintUserItemTemplate>();
        }

        void RegisterGraffitiTemplates() {
            TemplateRegistry.Register<GraffitiMarketItemTemplate>();
            TemplateRegistry.Register<ChildGraffitiMarketItemTemplate>();
            TemplateRegistry.Register<GraffitiUserItemTemplate>();
        }

        void RegisterSupplyTemplates() {
            TemplateRegistry.Register<SupplyMarketItemTemplate>();
            TemplateRegistry.Register<SupplyUserItemTemplate>();
            TemplateRegistry.Register<ArmorPropertyTemplate>();
            TemplateRegistry.Register<DamagePropertyTemplate>();
            TemplateRegistry.Register<MinePropertyTemplate>();
            TemplateRegistry.Register<RepairPropertyTemplate>();
            TemplateRegistry.Register<SpeedPropertyTemplate>();
            TemplateRegistry.Register<BuyButtonWithPriceTemplate>();
        }

        void RegisterPayableSystemsAndTemplates() {
            TemplateRegistry.Register<ChangeUIDTemplate>();
            EngineService.RegisterSystem(new ChangeUIDSystem());
        }
    }
}
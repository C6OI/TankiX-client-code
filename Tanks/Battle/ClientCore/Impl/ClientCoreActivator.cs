using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientCoreActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterWeaponTemplates();
            RegisterPaintTemplates();
            RegisterGraffitiTemplates();
            RegisterShellTemplates();
            RegisterSkinTemplates();
            EngineService.RegisterSystem(new FixedUpdateLimitSystem());
            EngineService.RegisterSystem(new FlagCollisionSystem());
            EngineService.RegisterSystem(new FlagBuilderSystem());
            EngineService.RegisterSystem(new FlagDropSystem());
            EngineService.RegisterSystem(new CTFBuilderSystem());
            EngineService.RegisterSystem(new SelfMarkerSystem());
            EngineService.RegisterSystem(new TankPhysicsSystem());
            EngineService.RegisterSystem(new TankExplosionSystem());
            EngineService.RegisterSystem(new ChassisSystem());
            EngineService.RegisterSystem(new TankCollidersSystem());
            EngineService.RegisterSystem(new InputManagerSystem());
            EngineService.RegisterSystem(new MouseControlSettingsSystem());

            if (!Application.isEditor) {
                EngineService.RegisterSystem(new CursorStateSystem());
            }

            EngineService.RegisterSystem(new PauseSystem());
            EngineService.RegisterSystem(new InstanceDestructionSystem());
            EngineService.RegisterSystem(new TankAssemblySystem());
            EngineService.RegisterSystem(new ClientHullBuilderSystem());
            EngineService.RegisterSystem(new PaintBuilderSystem());
            EngineService.RegisterSystem(new TankCollisionDetectionSystem());
            EngineService.RegisterSystem(new SelfDestructionControllerSystem());
            EngineService.RegisterSystem(new TankUpsideDownSystem());
            EngineService.RegisterSystem(new IdleKickSystem());
            EngineService.RegisterSystem(new TankEngineSystem());
            EngineService.RegisterSystem(new TankFallingSystem());
            AddMapSystems();
            AddWeaponSystems();
            AddBonusSystems();
            AddMineSystems();
            EngineService.RegisterSystem(new InventoryActivationSystem());
            EngineService.RegisterSystem(new LocalDurationSystem());
            EngineService.RegisterSystem(new PerformanceStatisticsSystem());
            EngineService.RegisterSystem(new BattleLabelSystem());
            EngineService.RegisterSystem(new BattleLabelLoadSystem());
            EngineService.RegisterSystem(new ServerShutdownNotificationSystem());
            EngineService.RegisterSystem(new BattleShutdownNotificationSystem());
            EngineService.RegisterSystem(new ShutdownScreenSystem());
            ServiceRegistry.Current.RegisterService(new BattleFlowInstancesCache());
        }

        void AddMapSystems() => EngineService.RegisterSystem(new MapLoaderSystem());

        void AddWeaponSystems() {
            EngineService.RegisterSystem(new SmokyBuilderSystem());
            EngineService.RegisterSystem(new ShaftBuilderSystem());
            EngineService.RegisterSystem(new FlamethrowerBuilderSystem());
            EngineService.RegisterSystem(new RicochetBuilderSystem());
            EngineService.RegisterSystem(new TwinsBuilderSystem());
            EngineService.RegisterSystem(new ThunderBuilderSystem());
            EngineService.RegisterSystem(new IsisBuilderSystem());
            EngineService.RegisterSystem(new MuzzlePointSystem());
            EngineService.RegisterSystem(new MuzzlePointSwitchSystem());
            EngineService.RegisterSystem(new CommonWeaponBuilderSystem());
            EngineService.RegisterSystem(new WeaponGyroscopeRotationSystem());
            EngineService.RegisterSystem(new WeaponRotationInputSystem());
            EngineService.RegisterSystem(new WeaponRotationSystem());
            EngineService.RegisterSystem(new WeaponCooldownSystem());
            EngineService.RegisterSystem(new WeaponHitSystem());
            EngineService.RegisterSystem(new WeaponShotSystem());
            EngineService.RegisterSystem(new ShotValidateSystem());
            EngineService.RegisterSystem(new SplashHitSystem());
            EngineService.RegisterSystem(new FreezeBuilderSystem());
            EngineService.RegisterSystem(new HammerBuilderSystem());
            EngineService.RegisterSystem(new HammerHitSystem());
            EngineService.RegisterSystem(new HammerTargetSectorCollectorSystem());
            EngineService.RegisterSystem(new LocalHammerMagazineSystem());
            EngineService.RegisterSystem(new DiscreteWeaponEnergySystem());
            EngineService.RegisterSystem(new DiscreteWeaponControllerSystem());
            EngineService.RegisterSystem(new StreamWeaponControllerSystem());
            EngineService.RegisterSystem(new StreamWeaponCooldownSystem());
            EngineService.RegisterSystem(new StreamWeaponEnergySystem());
            EngineService.RegisterSystem(new StreamHitSystem());
            EngineService.RegisterSystem(new StreamHitLoadCheckerSystem());
            EngineService.RegisterSystem(new BulletTargetingSystem());
            EngineService.RegisterSystem(new VerticalTargetingSystem());
            EngineService.RegisterSystem(new VerticalSectorTargetingSystem());
            EngineService.RegisterSystem(new ConicTargetingSystem());
            EngineService.RegisterSystem(new TargetSectorCollectorSystem());
            EngineService.RegisterSystem(new VerticalDirectionsCollectorSystem());
            EngineService.RegisterSystem(new SectorDirectionsCollectorSystem());
            EngineService.RegisterSystem(new ConicDirectionsCollectorSystem());
            EngineService.RegisterSystem(new TargetCollectorSystem());
            EngineService.RegisterSystem(new BulletTargetCollectorSystem());
            EngineService.RegisterSystem(new RicochetTargetCollectorSystem());
            EngineService.RegisterSystem(new HammerTargetCollectorSystem());
            EngineService.RegisterSystem(new DistanceAndAngleTargetEvaluatorSystem());
            EngineService.RegisterSystem(new TeamTargetEvaluatorSystem());
            EngineService.RegisterSystem(new CTFTargetEvaluatorSystem());
            EngineService.RegisterSystem(new DirectionEvaluatorSystem());
            EngineService.RegisterSystem(new DiscreteImpactSystem());
            EngineService.RegisterSystem(new ImpactWeakeningByTargetSystem());
            EngineService.RegisterSystem(new SplashImpactSystem());
            EngineService.RegisterSystem(new KickbackSystem());
            EngineService.RegisterSystem(new WeaponBulletShotSystem());
            EngineService.RegisterSystem(new BulletSystem());
            EngineService.RegisterSystem(new RicochetBulletSystem());
            EngineService.RegisterSystem(new TwinsBulletSystem());
            EngineService.RegisterSystem(new RailgunBuilderSystem());
            EngineService.RegisterSystem(new RailgunChargingWeaponControllerSystem());
            EngineService.RegisterSystem(new PenetrationTargetCollectorSystem());
            EngineService.RegisterSystem(new VulcanBuilderSystem());
            EngineService.RegisterSystem(new VulcanWeaponControllerSystem());
            EngineService.RegisterSystem(new VulcanGyroscopeSystem());
            EngineService.RegisterSystem(new VulcanImpactSystem());
            EngineService.RegisterSystem(new VulcanKickbackSystem());
            EngineService.RegisterSystem(new IsisSystem());
            EngineService.RegisterSystem(new IsisTargetEvaluatorSystem());
            EngineService.RegisterSystem(new ShaftStateControllerSystem());
            EngineService.RegisterSystem(new ShaftEnergySystem());
            EngineService.RegisterSystem(new ShaftAimingTargetPointSystem());
            EngineService.RegisterSystem(new ShaftHitSystem());
            EngineService.RegisterSystem(new ShaftAimingShotSystem());
            EngineService.RegisterSystem(new ShaftAimingStraightTargetingSystem());
            EngineService.RegisterSystem(new ShaftAimingStraightDirectionCollectorSystem());
            EngineService.RegisterSystem(new ShaftImpactSystem());
            EngineService.RegisterSystem(new ShaftAimingInputContextSystem());
            EngineService.RegisterSystem(new ShaftAimingVerticalTargetingControllerSystem());
            EngineService.RegisterSystem(new ShaftAimingCooldownSystem());
            EngineService.RegisterSystem(new ShaftAimingTargetCollectorSystem());
            EngineService.RegisterSystem(new ShaftShotValidateSystem());
            EngineService.RegisterSystem(new WeaponEnergySystem());
            EngineService.RegisterSystem(new ShotToHitSystem());
        }

        void AddMineSystems() {
            EngineService.RegisterSystem(new MineSystem());
            EngineService.RegisterSystem(new MineImpactSystem());
            EngineService.RegisterSystem(new MineFriendAndEnemySystem());
            EngineService.RegisterSystem(new MineActivateValidationSystem());
        }

        void AddBonusSystems() {
            EngineService.RegisterSystem(new BonusBuilderSystem());
            EngineService.RegisterSystem(new BonusClientConfigLoaderSystem());
            EngineService.RegisterSystem(new BonusSpawnSystem());
            EngineService.RegisterSystem(new BonusFallingSystem());
            EngineService.RegisterSystem(new BonusAlignmentToGroundSystem());
            EngineService.RegisterSystem(new BonusTakingSystem());
            EngineService.RegisterSystem(new BonusHidingSystem());
            EngineService.RegisterSystem(new BonusRegionLogicalBuilderSystem());
        }

        void RegisterTemplates() {
            TemplateRegistry.Register(typeof(BattleUserTemplate));
            TemplateRegistry.Register(typeof(RoundTemplate));
            TemplateRegistry.Register(typeof(RoundUserTemplate));
            TemplateRegistry.Register(typeof(TankTemplate));
            TemplateRegistry.Register(typeof(BonusTemplate));
            TemplateRegistry.Register(typeof(SupplyBonusTemplate));
            TemplateRegistry.Register(typeof(GoldBonusTemplate));
            TemplateRegistry.Register(typeof(BonusRegionTemplate));
            TemplateRegistry.Register(typeof(BonusRegionAssetsTemplate));
            TemplateRegistry.Register(typeof(MapTemplate));
            TemplateRegistry.Register(typeof(PerformanceStatisticsTemplate));
            TemplateRegistry.Register(typeof(GeneralBattleChatTemplate));
            TemplateRegistry.Register(typeof(TeamBattleChatTemplate));
        }

        void RegisterWeaponTemplates() {
            TemplateRegistry.Register(typeof(WeaponTemplate));
            TemplateRegistry.Register(typeof(StreamWeaponTemplate));
            TemplateRegistry.Register(typeof(DiscreteWeaponEnergyTemplate));
            TemplateRegistry.Register(typeof(SmokyBattleItemTemplate));
            TemplateRegistry.Register(typeof(ShaftBattleItemTemplate));
            TemplateRegistry.Register(typeof(FlamethrowerBattleItemTemplate));
            TemplateRegistry.Register(typeof(FreezeBattleItemTemplate));
            TemplateRegistry.Register(typeof(TwinsBattleItemTemplate));
            TemplateRegistry.Register(typeof(RicochetBattleItemTemplate));
            TemplateRegistry.Register(typeof(TwinsBulletTemplate));
            TemplateRegistry.Register(typeof(RicochetBulletTemplate));
            TemplateRegistry.Register(typeof(ThunderBattleItemTemplate));
            TemplateRegistry.Register(typeof(HammerBattleItemTemplate));
            TemplateRegistry.Register(typeof(RailgunBattleItemTemplate));
            TemplateRegistry.Register(typeof(VulcanBattleItemTemplate));
            TemplateRegistry.Register(typeof(IsisBattleItemTemplate));
        }

        void RegisterGraffitiTemplates() => TemplateRegistry.Register<GraffitiBattleItemTemplate>();

        void RegisterPaintTemplates() => TemplateRegistry.Register<PaintBattleItemTemplate>();

        void RegisterShellTemplates() => TemplateRegistry.Register<ShellBattleItemTemplate>();

        void RegisterSkinTemplates() {
            TemplateRegistry.Register<WeaponSkinBattleItemTemplate>();
            TemplateRegistry.Register<HullSkinBattleItemTemplate>();
        }

        protected override void Activate() {
            PhysicsSetup.CheckCollisionMatrix();
            ServiceRegistry.Current.RegisterService((InputManager)new InputManagerImpl());
        }
    }
}
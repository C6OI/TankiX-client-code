using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.API;
using tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientCoreActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterWeaponTemplates();
            RegisterReticleTemplates();
            RegisterPaintTemplates();
            RegisterGraffitiTemplates();
            RegisterShellTemplates();
            RegisterSkinTemplates();
            EngineService.RegisterSystem(new MapPhysicsSystem());
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
            EngineService.RegisterSystem(new TankJumpingSystem());
            EngineService.RegisterSystem(new NanFixerSystem());

            if (!Application.isEditor) {
                EngineService.RegisterSystem(new CursorStateSystem());
            }

            EngineService.RegisterSystem(new PauseSystem());
            EngineService.RegisterSystem(new InstanceDestructionSystem());
            EngineService.RegisterSystem(new TankIncarnationSystem());
            EngineService.RegisterSystem(new TankAssemblySystem());
            EngineService.RegisterSystem(new ClientHullBuilderSystem());
            EngineService.RegisterSystem(new PaintBuilderSystem());
            EngineService.RegisterSystem(new TankFriendAndEnemySystem());
            EngineService.RegisterSystem(new TankCollisionDetectionSystem());
            EngineService.RegisterSystem(new SelfDestructionControllerSystem());
            EngineService.RegisterSystem(new TankUpsideDownSystem());
            EngineService.RegisterSystem(new IdleKickSystem());
            EngineService.RegisterSystem(new TankEngineSystem());
            EngineService.RegisterSystem(new TankFallingSystem());
            EngineService.RegisterSystem(new HitFeedbackSystem());
            EngineService.RegisterSystem(new HealthSystem());
            EngineService.RegisterSystem(new TutorialHitTriggersSystem());
            EngineService.RegisterSystem(new InBattlesTutorialHandlersSystem());
            AddMapSystems();
            AddWeaponSystems();
            AddBonusSystems();
            AddMineSystems();
            AddModuleSystems();
            EngineService.RegisterSystem(new LocalDurationSystem());
            EngineService.RegisterSystem(new PerformanceStatisticsSystem());
            EngineService.RegisterSystem(new BattleStatisticsSystem());
            EngineService.RegisterSystem(new BattleLabelSystem());
            EngineService.RegisterSystem(new BattleLabelLoadSystem());
            EngineService.RegisterSystem(new ServerShutdownNotificationSystem());
            EngineService.RegisterSystem(new BattleShutdownNotificationSystem());
            EngineService.RegisterSystem(new ShutdownScreenSystem());
            EngineService.RegisterSystem(new BattlePingSystem());
            EngineService.RegisterSystem(new KeyboardSettingsSystem());
            EngineService.RegisterSystem(new AdminUserSystem());
            EngineService.RegisterSystem(new TesterUserSystem());
            RegisterEffects();
            ServiceRegistry.Current.RegisterService(new BattleFlowInstancesCache());
            EngineService.RegisterSystem(new ColorInBattleSystem());
        }

        void RegisterEffects() {
            TemplateRegistry.Register<ArmorEffectTemplate>();
            TemplateRegistry.Register<DamageEffectTemplate>();
            TemplateRegistry.Register<TemperatureEffectTemplate>();
            TemplateRegistry.Register<HealingEffectTemplate>();
            TemplateRegistry.Register<ForceFieldEffectTemplate>();
            TemplateRegistry.Register<OverhaulEffectTemplate>();
            TemplateRegistry.Register<HolyshieldEffectTemplate>();
            TemplateRegistry.Register<InvulnerabilityEffectTemplate>();
            TemplateRegistry.Register<SonarEffectTemplate>();
            TemplateRegistry.Register<InvisibilityEffectTemplate>();
            TemplateRegistry.Register<LifestealEffectTemplate>();
            TemplateRegistry.Register<RageEffectTemplate>();
            TemplateRegistry.Register<TurboSpeedEffectTemplate>();
            TemplateRegistry.Register<BurningEffectTemplate>();
            TemplateRegistry.Register<MineEffectTemplate>();
            TemplateRegistry.Register<IcetrapEffectTemplate>();
            TemplateRegistry.Register<AcceleratedGearsEffectTemplate>();
            TemplateRegistry.Register<JumpEffectTemplate>();
            TemplateRegistry.Register<SapperEffectTemplate>();
            TemplateRegistry.Register<EmergencyProtectionHealingPartEffectTemplate>();
            TemplateRegistry.Register<EmergencyProtectionEffectTemplate>();
            TemplateRegistry.Register<EMPEffectTemplate>();
            TemplateRegistry.Register<BackhitDefenceEffectTemplate>();
            TemplateRegistry.Register<BackhitIncreaseEffectTemplate>();
            TemplateRegistry.Register<EnergyInjectionEffectTemplate>();
            TemplateRegistry.Register<NormalizeTemperatureEffectTemplate>();
            TemplateRegistry.Register<TargetFocusEffectTemplate>();
            TemplateRegistry.Register<EngineerEffectTemplate>();
            TemplateRegistry.Register<AdrenalineEffectTemplate>();
            EngineService.RegisterSystem(new TargetFocusEffectSystem());
            EngineService.RegisterSystem(new EMPEffectSystem());
            EngineService.RegisterSystem(new EnergyInjectionEffectSystem());
            TemplateRegistry.Register<ExternalImpactEffectTemplate>();
            EngineService.RegisterSystem(new ExternalImpactEffectSystem());
            TemplateRegistry.Register<FireRingEffectTemplate>();
            EngineService.RegisterSystem(new FireRingEffectSystem());
            TemplateRegistry.Register<ExplosiveMassEffectTemplate>();
            EngineService.RegisterSystem(new ExplosiveMassEffectSystem());
            TemplateRegistry.Register<KamikadzeEffectTemplate>();
            EngineService.RegisterSystem(new KamikadzeEffectSystem());
        }

        void AddMapSystems() {
            EngineService.RegisterSystem(new MapLoaderSystem());
        }

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
            EngineService.RegisterSystem(new RailgunTargetingSystem());
            EngineService.RegisterSystem(new VulcanBuilderSystem());
            EngineService.RegisterSystem(new VulcanWeaponControllerSystem());
            EngineService.RegisterSystem(new VulcanGyroscopeSystem());
            EngineService.RegisterSystem(new VulcanImpactSystem());
            EngineService.RegisterSystem(new VulcanKickbackSystem());
            EngineService.RegisterSystem(new FreezeTargetingSystem());
            EngineService.RegisterSystem(new FlamethrowerTargetingSystem());
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

        void AddModuleSystems() {
            EngineService.RegisterSystem(new ModuleEffectSystem());
            EngineService.RegisterSystem(new UnitSystem());
            EngineService.RegisterSystem(new UnitMoveSystem());
            TemplateRegistry.Register(typeof(SpiderEffectTemplate));
            EngineService.RegisterSystem(new SpiderMineSystem());
            EngineService.RegisterSystem(new SpiderDirectionCollectorSystem());
            EngineService.RegisterSystem(new InventorySlotTemporaryBlockSystem());
            TemplateRegistry.Register(typeof(DroneEffectTemplate));
            TemplateRegistry.Register(typeof(DroneWeaponTemplate));
            EngineService.RegisterSystem(new DroneEffectSystem());
            EngineService.RegisterSystem(new DroneWeaponSystem());
            EngineService.RegisterSystem(new DroneMovementSystem());
            EngineService.RegisterSystem(new JumpImpactSystem());
        }

        void AddMineSystems() {
            EngineService.RegisterSystem(new MineSystem());
            EngineService.RegisterSystem(new CommonMineSystem());
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
            TemplateRegistry.Register(typeof(TankIncarnationTemplate));
            TemplateRegistry.Register(typeof(BonusTemplate));
            TemplateRegistry.Register(typeof(SupplyBonusTemplate));
            TemplateRegistry.Register(typeof(GoldBonusTemplate));
            TemplateRegistry.Register(typeof(GoldBonusWithCrystalsTemplate));
            TemplateRegistry.Register(typeof(GoldBonusWithTeleportTemplate));
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

        void RegisterReticleTemplates() {
            TemplateRegistry.Register<ReticleTemplate>();
        }

        void RegisterGraffitiTemplates() {
            TemplateRegistry.Register<GraffitiBattleItemTemplate>();
        }

        void RegisterPaintTemplates() {
            TemplateRegistry.Register<TankPaintBattleItemTemplate>();
            TemplateRegistry.Register<WeaponPaintBattleItemTemplate>();
        }

        void RegisterShellTemplates() {
            TemplateRegistry.Register<ShellBattleItemTemplate>();
        }

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
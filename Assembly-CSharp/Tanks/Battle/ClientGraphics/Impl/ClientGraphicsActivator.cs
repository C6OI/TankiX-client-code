using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ClientGraphicsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        public static GUIStyle guiStyle;

        public static readonly string QUALITY_SETTINGS_ROOT_CONFIG_PATH = "clientlocal/graphics/settings/quality/";

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterSystems();
        }

        protected override void Activate() {
            CreateGUIStyle();
            CreateQualitySettingsEntity();
            AdjustTargetMaxTextureSize();
        }

        void AdjustTargetMaxTextureSize() {
            if (SystemInfo.graphicsMemorySize <= 128) {
                QualitySettings.masterTextureLimit = 3;
            } else if (SystemInfo.graphicsMemorySize <= 256) {
                QualitySettings.masterTextureLimit = 2;
            } else if (SystemInfo.graphicsMemorySize <= 512) {
                QualitySettings.masterTextureLimit = 2;
            }
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new MapEffectSystem());
            EngineService.RegisterSystem(new TankAnimationSystem());
            EngineService.RegisterSystem(new NitroEffectSystem());
            EngineService.RegisterSystem(new DoubleDamageEffectSystem());
            EngineService.RegisterSystem(new ShotAnimationSystem());
            EngineService.RegisterSystem(new AcceleratedGearsEffectSystem());
            EngineService.RegisterSystem(new ChassisAnimationInitSystem());
            EngineService.RegisterSystem(new ChassisAnimationSystem());
            EngineService.RegisterSystem(new CommonDecalHitSystem());
            EngineService.RegisterSystem(new ShaftDecalHitSystem());
            EngineService.RegisterSystem(new StreamingDecalHitSystem());
            EngineService.RegisterSystem(new HammerDecalHitSystem());
            EngineService.RegisterSystem(new GraffitiDecalSystem());
            EngineService.RegisterSystem(new TrackMarksSystem());
            EngineService.RegisterSystem(new TrackDustSystem());
            EngineService.RegisterSystem(new CartridgeCaseLifeSystem());
            EngineService.RegisterSystem(new FlagTransparencySystem());
            EngineService.RegisterSystem(new FlagVisualDropSystem());
            EngineService.RegisterSystem(new FlagVisualPickupSystem());
            EngineService.RegisterSystem(new TransformTimeSmoothingSystem());
            EngineService.RegisterSystem(new ClientTankSnapSystem());
            EngineService.RegisterSystem(new RemoteTankSmootherSystem());
            EngineService.RegisterSystem(new RemoteWeaponSmootherSystem());
            EngineService.RegisterSystem(new TankVisualAssemblySystem());
            EngineService.RegisterSystem(new FollowCameraSystem());
            EngineService.RegisterSystem(new FreeCameraSystem());
            EngineService.RegisterSystem(new GoCameraSystem());
            EngineService.RegisterSystem(new BattleCameraBuilderSystem());
            EngineService.RegisterSystem(new ApplyCameraTransformSystem());
            EngineService.RegisterSystem(new CameraFOVUpdateSystem());
            EngineService.RegisterSystem(new MouseOrbitCameraSystem());
            EngineService.RegisterSystem(new TransitionCameraSystem());
            EngineService.RegisterSystem(new EffectArrangementSystem());
            EngineService.RegisterSystem(new HullBuilderGraphicsSystem());
            EngineService.RegisterSystem(new PaintGraphicsBuilderSystem());
            EngineService.RegisterSystem(new TankCommonBuilderSystem());
            EngineService.RegisterSystem(new BrokenTankEffectSystem());
            EngineService.RegisterSystem(new MuzzlePointGraphicsSystem());
            EngineService.RegisterSystem(new DoubleArmorEffectSystem());
            EngineService.RegisterSystem(new CommonWeaponVisualBuilderSystem());
            EngineService.RegisterSystem(new TankRendererSystem());
            EngineService.RegisterSystem(new TankActiveStateSystem());
            EngineService.RegisterSystem(new TankDeadStateSystem());
            EngineService.RegisterSystem(new TankSemiActiveStateSystem());
            EngineService.RegisterSystem(new TransparencyTransitionSystem());
            EngineService.RegisterSystem(new MuzzleFlashSystem());
            EngineService.RegisterSystem(new RailgunChargingEffectSystem());
            EngineService.RegisterSystem(new RailgunTrailSystem());
            EngineService.RegisterSystem(new BulletGraphicsSystem());
            EngineService.RegisterSystem(new ModuleEffectGraphicsSystem());
            EngineService.RegisterSystem(new MineCommonGraphicsSystem());
            EngineService.RegisterSystem(new MineGraphicsSystem());
            EngineService.RegisterSystem(new SpiderMineVisualSystem());
            EngineService.RegisterSystem(new TankExplosionGraphicsSystem());
            EngineService.RegisterSystem(new HitExplosionGraphicsSystem());
            EngineService.RegisterSystem(new CriticalDamageGraphicsSystem());
            EngineService.RegisterSystem(new SoleTracerGraphicsSystem());
            EngineService.RegisterSystem(new PelletThrowingGraphicsSystem());
            EngineService.RegisterSystem(new IsisGraphicsSystem());
            EngineService.RegisterSystem(new IsisWorkingAnimationSystem());
            EngineService.RegisterSystem(new StreamWeaponParticleSystem());
            EngineService.RegisterSystem(new WeaponStreamMuzzleFlashSystem());
            EngineService.RegisterSystem(new WeaponStreamTracerSystem());
            EngineService.RegisterSystem(new VulcanTurbineAnimationSystem());
            EngineService.RegisterSystem(new VulcanBandAnimationSystem());
            EngineService.RegisterSystem(new WeaponStreamHitGraphicsSystem());
            EngineService.RegisterSystem(new RicochetBulletBounceGraphicsSystem());
            EngineService.RegisterSystem(new TankVisualTemperatureControllerSystem());
            EngineService.RegisterSystem(new ShaftAimingCameraSystem());
            EngineService.RegisterSystem(new ShaftAimingMapEffectSystem());
            EngineService.RegisterSystem(new ShaftAimingReticleEffectSystem());
            EngineService.RegisterSystem(new ShaftAimingRendererSystem());
            EngineService.RegisterSystem(new RemoteShaftAimingLaserSystem());
            EngineService.RegisterSystem(new ShaftAimingColorEffectSystem());
            EngineService.RegisterSystem(new ShaftAimingForceFieldEffectSystem());
            EngineService.RegisterSystem(new ShaftAimingAnimationSystem());
            EngineService.RegisterSystem(new ShaftShotAnimationSystem());
            EngineService.RegisterSystem(new MapInitSystem());
            EngineService.RegisterSystem(new PoolContainerSystem());
            EngineService.RegisterSystem(new CameraInitSystem());
            EngineService.RegisterSystem(new TankPartIntersectionWithCameraSystem());
            EngineService.RegisterSystem(new RailgunAnimationSystem());
            EngineService.RegisterSystem(new FreezeMotionAnimationSystem());
            EngineService.RegisterSystem(new FlamethrowerMotionAnimationSystem());
            EngineService.RegisterSystem(new TwinsAnimationSystem());
            EngineService.RegisterSystem(new CartridgeCaseEjectionSystem());
            EngineService.RegisterSystem(new HammerShotAnimationSystem());
            EngineService.RegisterSystem(new MapHidingGeometryCollectorSystem());
            EngineService.RegisterSystem(new BonusGraphicsBuilderSystem());
            EngineService.RegisterSystem(new BonusBoxAppearingSystem());
            EngineService.RegisterSystem(new BonusBoxDisappearingSystem());
            EngineService.RegisterSystem(new BonusParachuteAppearingSystem());
            EngineService.RegisterSystem(new BonusParachuteDisappearingSystem());
            EngineService.RegisterSystem(new BonusRegionShowSystem());
            EngineService.RegisterSystem(new BonusRegionVisualBuilderSystem());
            EngineService.RegisterSystem(new GoldRegionShowSystem());
            EngineService.RegisterSystem(new SupplyRegionShowSystem());
            EngineService.RegisterSystem(new SaveLoadCameraSystem());
            EngineService.RegisterSystem(new SpectatorCameraManagerSystem());
            EngineService.RegisterSystem(new UpdateUserRankEffectSystem());
            EngineService.RegisterSystem(new UpdateUserRankSoundEffectSystem());
            EngineService.RegisterSystem(new TankInvisibilityEffectSystem());
            EngineService.RegisterSystem(new SonarEffectSystem());
            EngineService.RegisterSystem(new TankShaderSystem());
            EngineService.RegisterSystem(new HealingGraphicsEffectSystem());
            EngineService.RegisterSystem(new CameraShakerSystem());
            EngineService.RegisterSystem(new HealthFeedbackGraphicsSystem());
            EngineService.RegisterSystem(new EmergencyProtectionGraphicsEffectSystem());
            EngineService.RegisterSystem(new EMPGraphicsEffectSystem());
            EngineService.RegisterSystem(new LifestealGraphicsEffectSystem());
            EngineService.RegisterSystem(new HolyshieldGraphicsEffectSystem());
            EngineService.RegisterSystem(new ForceFieldEffectSystem());
            EngineService.RegisterSystem(new ForceFieldSlotActivationValidatorSystem());
            EngineService.RegisterSystem(new DroneGraphicsSystem());
            EngineService.RegisterSystem(new CustomTankBuilderSystem());
            EngineService.RegisterSystem(new ParticleSystemEffectsSystem());
            EngineService.RegisterSystem(new ModuleVisualEffectSystem());
        }

        void RegisterTemplates() {
            TemplateRegistry.Register(typeof(CameraTemplate));
            TemplateRegistry.Register(typeof(QualitySettingsTemplate));
            TemplateRegistry.RegisterPart<DiscreteWeaponCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<ShaftCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<VulcanCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<SpiderMineCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<MineCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<HammerHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<RailgunHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<RicochetHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<ShaftHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<SmokyHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<ThunderHitFeedbackSoundsTemplatePart>();
        }

        void CreateGUIStyle() {
            guiStyle = new GUIStyle();
            Texture2D background = CreateColoredTexture(0f, 0f, 0f, 0.4f);
            guiStyle.normal.background = background;
            guiStyle.normal.textColor = new Color(255f, 255f, 255f, 0.7f);
            guiStyle.alignment = TextAnchor.UpperLeft;
            guiStyle.fontSize = 30;
            guiStyle.padding = new RectOffset(20, 5, 5, 5);
            guiStyle.font = (Font)Resources.Load("SEGUISYM");
        }

        Texture2D CreateColoredTexture(float r, float g, float b, float a) {
            Texture2D texture2D = new(1, 1);
            Color color = new(r, g, b, a);
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            return texture2D;
        }

        public void CreateQualitySettingsEntity() {
            string text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            text = text.Replace(" ", string.Empty).ToLower();
            string configPath = QUALITY_SETTINGS_ROOT_CONFIG_PATH + text;
            EngineService.Engine.CreateEntity<QualitySettingsTemplate>(configPath);
        }
    }
}
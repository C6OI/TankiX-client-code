using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ClientSoundActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [SerializeField] int minProcessorCount = 2;

        [SerializeField] int maxRealVoicesCountForWeakCPU = 32;

        [SerializeField] int[] maxRealVoicesByQualityIndex;

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new ModuleEffectSoundSystem());
            EngineService.RegisterSystem(new HolyshieldSoundEffectSystem());
            EngineService.RegisterSystem(new DroneFlySoundEffectSystem());
            EngineService.RegisterSystem(new WeaponStreamHitSoundsSystem());
            EngineService.RegisterSystem(new EnergyInjectionSoundEffectSystem());
            EngineService.RegisterSystem(new RageSoundEffectSystem());
            EngineService.RegisterSystem(new TankExplosionSoundSystem());
            EngineService.RegisterSystem(new HullSoundBuilderSystem());
            EngineService.RegisterSystem(new WeaponSoundRotationSystem());
            EngineService.RegisterSystem(new DiscreteWeaponShotEffectSystem());
            EngineService.RegisterSystem(new StreamWeaponSoundEffectSystem());
            EngineService.RegisterSystem(new MagazineSoundEffectSystem());
            EngineService.RegisterSystem(new RailgunShotEffectSystem());
            EngineService.RegisterSystem(new CTFSoundsSystem());
            EngineService.RegisterSystem(new VulcanSoundEffectSystem());
            EngineService.RegisterSystem(new HitExplosionSoundSystem());
            EngineService.RegisterSystem(new MineCommonSoundsSystem());
            EngineService.RegisterSystem(new IceTrapSoundsSystem());
            EngineService.RegisterSystem(new SpiderMineSoundsSystem());
            EngineService.RegisterSystem(new BonusTakingSoundSystem());
            EngineService.RegisterSystem(new GoldNotificationSoundSystem());
            EngineService.RegisterSystem(new ShaftAimingSoundEffectSystem());
            EngineService.RegisterSystem(new CaseSoundEffectSystem());
            EngineService.RegisterSystem(new RicochetSoundEffectSystem());
            EngineService.RegisterSystem(new IsisSoundEffectSystem());
            EngineService.RegisterSystem(new ShaftHitSoundEffectSystem());
            EngineService.RegisterSystem(new ShaftShotSoundEffectSystem());
            EngineService.RegisterSystem(new HammerHitSoundEffectSystem());
            EngineService.RegisterSystem(new AmbientMapSoundEffectSystem());
            EngineService.RegisterSystem(new AmbientZoneSoundEffectSystem());
            EngineService.RegisterSystem(new MapNativeSoundsSystem());
            EngineService.RegisterSystem(new SoundListenerSystem());
            EngineService.RegisterSystem(new TankFallingSoundEffectSystem());
            EngineService.RegisterSystem(new TankEngineSoundEffectSystem());
            EngineService.RegisterSystem(new TankFrictionSoundSystem());
            EngineService.RegisterSystem(new SoundListenerStateSystem());
            EngineService.RegisterSystem(new KillTankSoundSystem());
            EngineService.RegisterSystem(new BattleSoundsSystem());
            EngineService.RegisterSystem(new SoundListenerBattleSnapshotsSystem());
            EngineService.RegisterSystem(new SoundListenerCleanerSystem());
            EngineService.RegisterSystem(new HitFeedbackSoundSystem());
            EngineService.RegisterSystem(new HealthFeedbackSoundSystem());
            EngineService.RegisterSystem(new WeaponEnergyFeedbackSoundSystem());
            EngineService.RegisterSystem(new MapAnimatorTimerSystem());
            EngineService.RegisterSystem(new LazySkyboxLoadingSystem());
            EngineService.RegisterSystem(new TankJumpSoundSystem());
        }

        protected override void Activate() {
            UpdateAudioConfiguration();
        }

        void OnAudioConfigurationChanged(bool deviceWasChanged) {
            if (deviceWasChanged) {
                UpdateAudioConfiguration();
            }
        }

        void UpdateAudioConfiguration() {
            AudioConfiguration configuration = AudioSettings.GetConfiguration();
            int processorCount = SystemInfo.processorCount;

            if (processorCount <= minProcessorCount) {
                configuration.numRealVoices = maxRealVoicesCountForWeakCPU;
            } else {
                int currentQualityLevel = GraphicsSettings.INSTANCE.CurrentQualityLevel;
                int numRealVoices = maxRealVoicesByQualityIndex[currentQualityLevel];
                configuration.numRealVoices = numRealVoices;
            }

            AudioSettings.Reset(configuration);
        }
    }
}
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleHudRootComponent : MonoBehaviour {
        public CameraShaker shaker;

        bool ValidateShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) => settings.Enabled && cameraShakerConfig.Enabled;

        public void ShakeHUDOnFalling(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent config) {
            if (ValidateShake(settings, config)) {
                CameraShakeInstance cameraShakeInstance = shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime,
                    new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnImpact(GameCameraShakerSettingsComponent settings, ImpactCameraShakerConfigComponent config) {
            if (ValidateShake(settings, config)) {
                CameraShakeInstance cameraShakeInstance = shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime,
                    new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnDeath(GameCameraShakerSettingsComponent settings, TankCameraShakerConfigOnDeathComponent config) {
            if (ValidateShake(settings, config)) {
                CameraShakeInstance cameraShakeInstance = shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime,
                    new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnShot(GameCameraShakerSettingsComponent settings, KickbackCameraShakerConfigComponent config) {
            if (ValidateShake(settings, config)) {
                CameraShakeInstance cameraShakeInstance = shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime,
                    new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }
    }
}
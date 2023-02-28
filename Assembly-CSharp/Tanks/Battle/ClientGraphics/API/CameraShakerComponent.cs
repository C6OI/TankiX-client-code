using Tanks.Lobby.ClientProfile.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraShakerComponent : Component {
        const float MIN_FRONT_IMPACT_LENGTH = 0.005f;

        readonly CameraShaker cameraShaker;

        public CameraShakerComponent(CameraShaker cameraShaker) => this.cameraShaker = cameraShaker;

        public void ShakeByDiscreteImpact(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, Vector3 impactFrontDirectionWorldSpace,
            float weakeningCoeff) {
            if (ValidateShake(settings, cameraShakerConfig) && !(impactFrontDirectionWorldSpace.magnitude < 0.005f)) {
                ImpactAlignedInfluence impactAlignedInfluence = CalculateInfluence(impactFrontDirectionWorldSpace, cameraShakerConfig);

                CameraShakeInstance cameraShakeInstance = cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime,
                    cameraShakerConfig.FadeOutTime, impactAlignedInfluence.posInfluence, impactAlignedInfluence.rotInfluence);

                cameraShakeInstance.ScaleMagnitude = weakeningCoeff;
            }
        }

        public void ShakeByFalling(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, float weakeningCoeff) {
            if (ValidateShake(settings, cameraShakerConfig)) {
                CameraShakeInstance cameraShakeInstance = cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime,
                    cameraShakerConfig.FadeOutTime, new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ),
                    new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));

                cameraShakeInstance.ScaleMagnitude = weakeningCoeff;
            }
        }

        ImpactAlignedInfluence CalculateInfluence(Vector3 impactFrontDirectionWorldSpace, CameraShakerConfigComponent cameraShakerConfig) {
            Vector3 up = cameraShaker.transform.up;
            float magnitude = Vector3.Cross(impactFrontDirectionWorldSpace, up).magnitude;
            Vector3 vector = !(magnitude > 0.01f) ? cameraShaker.transform.right : up;
            vector = Vector3.Normalize(vector - impactFrontDirectionWorldSpace * Vector3.Dot(vector, impactFrontDirectionWorldSpace));
            Vector3 vector2 = Vector3.Cross(vector, impactFrontDirectionWorldSpace);
            Matrix4x4 matrix4x = default;
            Matrix4x4 worldToLocalMatrix = cameraShaker.transform.worldToLocalMatrix;
            matrix4x.SetColumn(0, new Vector4(vector2.x, vector2.y, vector2.z, 0f));
            matrix4x.SetColumn(1, new Vector4(vector.x, vector.y, vector.z, 0f));
            matrix4x.SetColumn(2, new Vector4(impactFrontDirectionWorldSpace.x, impactFrontDirectionWorldSpace.y, impactFrontDirectionWorldSpace.z, 0f));
            matrix4x.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            Matrix4x4 matrix4x2 = worldToLocalMatrix * matrix4x;
            Vector3 posInfluence = matrix4x2.MultiplyVector(new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ));
            Vector3 rotInfluence = matrix4x2.MultiplyVector(new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
            return new ImpactAlignedInfluence(posInfluence, rotInfluence);
        }

        bool ValidateShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) => settings.Enabled && cameraShakerConfig.Enabled;

        public void ShakeOnce(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) {
            if (ValidateShake(settings, cameraShakerConfig)) {
                cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, cameraShakerConfig.FadeOutTime,
                    new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ),
                    new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
            }
        }

        public void ShakeOnce(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, float cooldownTime) {
            if (!ValidateShake(settings, cameraShakerConfig)) {
                return;
            }

            float num = cameraShakerConfig.FadeInTime;
            float num2 = cameraShakerConfig.FadeOutTime;

            if (num + num2 > cooldownTime) {
                if (num > cooldownTime) {
                    num = 0f;
                    num2 = cooldownTime;
                } else {
                    num2 = cooldownTime - num;
                }
            }

            cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, num, num2,
                new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ),
                new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
        }

        public CameraShakeInstance StartShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) {
            if (!ValidateShake(settings, cameraShakerConfig)) {
                return null;
            }

            CameraShakeInstance cameraShakeInstance = cameraShaker.StartShake(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime,
                new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ),
                new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));

            cameraShakeInstance.deleteOnInactive = false;
            return cameraShakeInstance;
        }

        public CameraShakeInstance UpdateImpactShakeInstance(GameCameraShakerSettingsComponent settings, CameraShakeInstance instance, CameraShakerConfigComponent cameraShakerConfig,
            Vector3 frontDir, float weakening) {
            if (!ValidateShake(settings, cameraShakerConfig)) {
                return instance;
            }

            if (frontDir.magnitude < 0.005f) {
                return instance;
            }

            ImpactAlignedInfluence impactAlignedInfluence = CalculateInfluence(frontDir, cameraShakerConfig);

            if (instance == null) {
                instance = cameraShaker.StartShake(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, impactAlignedInfluence.posInfluence,
                    impactAlignedInfluence.rotInfluence);

                instance.deleteOnInactive = false;
            }

            instance.ScaleMagnitude = weakening;
            instance.positionInfluence = impactAlignedInfluence.posInfluence;
            instance.rotationInfluence = impactAlignedInfluence.rotInfluence;
            return instance;
        }

        struct ImpactAlignedInfluence {
            public readonly Vector3 posInfluence;

            public readonly Vector3 rotInfluence;

            public ImpactAlignedInfluence(Vector3 posInfluence, Vector3 rotInfluence) {
                this.posInfluence = posInfluence;
                this.rotInfluence = rotInfluence;
            }
        }
    }
}
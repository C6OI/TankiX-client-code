using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class TankOutlineMapEffectComponent : BehaviourComponent {
        const string GLOBAL_OUTLINE_EFFECT_NAME = "_GlobalTankOutlineEffectAlpha";

        const string WORLD_SPACE_CENTER_NAME = "_WorldSpaceEffectCenter";

        const string TANK_OUTLINE_EFFECT_RADIUS = "_TankOutlineEffectRadius";

        const string GLOBAL_WORKING_COEFF = "_WorkingOutlineCoeff";

        const string GLOBAL_TANK_OUTLINE_MULTIPLIER = "_GlobalTankOutlineScaleMultiplier";

        public static bool IS_OUTLINE_EFFECT_RUNNING;

        [SerializeField] float maxEffectRadius = 3000f;

        [SerializeField] float globalRadiusTime = 15f;

        [SerializeField] float globalAlphaFadeInTime = 0.5f;

        [SerializeField] float globalAlphaFadeOutTime = 1.5f;

        [SerializeField] float minAlphaWhileBlinking = 0.25f;

        [SerializeField] float enterSorkingStateSoundDelay = 1f;

        [SerializeField] float workingFadeTimeOffset;

        [SerializeField] float generalBlinkTime = 20f;

        [SerializeField] float maxBlinkIterationTime = 2f;

        [SerializeField] float pauseWhenBlinkOnMaxAlpha = -1f;

        [SerializeField] float pauseWhenBlinkOnMinAlpha = 0.1f;

        [SerializeField] float radarSoundInterval = 1f;

        [SerializeField] float radarFadeSoundDelay = 0.5f;

        [SerializeField] float workingStateFadeTime;

        [SerializeField] float blinkRadius = 1.1f;

        [SerializeField] SoundController radarSplashSound;

        [SerializeField] SoundController radarFadeSound;

        [SerializeField] SoundController deactivationRadarSound;

        [SerializeField] SoundController activationRadarSound;

        float alphaSpeedFadeIn;

        float alphaSpeedFadeOut;

        bool blinkForward;

        float blinkSpeed;

        float blinkTimer;

        Transform effectCenterTransform;

        bool enterWorkingState;

        float globalOutlineEffectAlpha;

        int globalOutlineEffectAlphaID;

        float globalOutlineEffectRadius;

        float globalWorkingCoeff;

        int globalWorkingCoeffID;

        Entity mapEffectEntity;

        float pauseTimer;

        States previousState;

        float radiusSpeed;

        States state;

        int tankOutlineEffectRadiusID;

        bool workingFadePause;

        float workingStateTimer;

        int worldSpaceEffectCenterID;

        float GlobalOutlineEffectRadius {
            get => globalOutlineEffectRadius;
            set {
                globalOutlineEffectRadius = Mathf.Clamp(value, 0f, maxEffectRadius);
                Shader.SetGlobalFloat(tankOutlineEffectRadiusID, globalOutlineEffectRadius);
            }
        }

        float GlobalWorkingCoeff {
            get => globalWorkingCoeff;
            set {
                globalWorkingCoeff = Mathf.Clamp01(value);
                Shader.SetGlobalFloat(globalWorkingCoeffID, globalWorkingCoeff);
            }
        }

        float GlobalOutlineEffectAlpha {
            get => globalOutlineEffectAlpha;
            set {
                globalOutlineEffectAlpha = Mathf.Clamp01(value);
                Shader.SetGlobalFloat(globalOutlineEffectAlphaID, globalOutlineEffectAlpha);
            }
        }

        States State {
            get => state;
            set {
                previousState = state;
                state = value;
                GlobalWorkingCoeff = 0f;

                switch (state) {
                    case States.IDLE:
                        GlobalOutlineEffectAlpha = 0f;
                        GlobalOutlineEffectRadius = 0f;
                        IS_OUTLINE_EFFECT_RUNNING = false;
                        enabled = false;
                        activationRadarSound.FadeOut();
                        StopRadarSounds();
                        SwitchEntityState<TankOutlineMapEffectStates.IdleState>();
                        break;

                    case States.ACTIVATION:
                        UpdateEffectTransformCenter();
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        enabled = true;
                        deactivationRadarSound.FadeOut();
                        activationRadarSound.SetSoundActive();
                        StopRadarSounds();
                        SwitchEntityState<TankOutlineMapEffectStates.ActivationState>();
                        break;

                    case States.WORKING:
                        workingFadePause = false;
                        enterWorkingState = true;
                        workingStateTimer = enterSorkingStateSoundDelay;
                        GlobalOutlineEffectAlpha = 1f;
                        GlobalOutlineEffectRadius = maxEffectRadius;
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        enabled = true;
                        deactivationRadarSound.FadeOut();
                        StopRadarSounds();
                        SwitchEntityState<TankOutlineMapEffectStates.WorkingState>();
                        break;

                    case States.BLINKER:
                        if (previousState == States.ACTIVATION && GlobalOutlineEffectAlpha < 1f) {
                            DeactivateEffect();
                            break;
                        }

                        IS_OUTLINE_EFFECT_RUNNING = true;
                        blinkTimer = 0f;
                        pauseTimer = -1f;
                        blinkForward = false;
                        enabled = true;
                        activationRadarSound.FadeOut();
                        deactivationRadarSound.SetSoundActive();
                        StopRadarSounds();
                        SwitchEntityState<TankOutlineMapEffectStates.BlinkerState>();
                        break;

                    case States.DEACTIVATION:
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        enabled = true;
                        activationRadarSound.FadeOut();
                        deactivationRadarSound.SetSoundActive();
                        StopRadarSounds();
                        SwitchEntityState<TankOutlineMapEffectStates.DeactivationState>();
                        break;
                }
            }
        }

        void Update() {
            float deltaTime = Time.deltaTime;

            switch (State) {
                case States.ACTIVATION:
                    GlobalOutlineEffectAlpha += alphaSpeedFadeIn * deltaTime;
                    GlobalOutlineEffectRadius += radiusSpeed * deltaTime;

                    if (GlobalOutlineEffectAlpha >= 1f && GlobalOutlineEffectRadius >= maxEffectRadius) {
                        State = States.WORKING;
                    }

                    break;

                case States.DEACTIVATION:
                    GlobalOutlineEffectAlpha -= alphaSpeedFadeOut * deltaTime;

                    if (GlobalOutlineEffectAlpha <= 0f) {
                        State = States.IDLE;
                    }

                    break;

                case States.BLINKER: {
                    GlobalOutlineEffectRadius += radiusSpeed * deltaTime;
                    pauseTimer -= deltaTime;

                    if (pauseTimer > 0f) {
                        break;
                    }

                    blinkTimer += deltaTime;

                    if (blinkTimer >= generalBlinkTime) {
                        GlobalOutlineEffectAlpha = 1f;
                        State = States.DEACTIVATION;
                        break;
                    }

                    float num2 = 2f * (1f - minAlphaWhileBlinking) / maxBlinkIterationTime;

                    if (blinkForward) {
                        GlobalOutlineEffectAlpha += num2 * deltaTime;

                        if (GlobalOutlineEffectAlpha >= 1f) {
                            GlobalOutlineEffectAlpha = 1f;
                            pauseTimer = pauseWhenBlinkOnMaxAlpha;
                            blinkForward = false;
                        }
                    } else {
                        GlobalOutlineEffectAlpha -= num2 * deltaTime;

                        if (GlobalOutlineEffectAlpha <= minAlphaWhileBlinking) {
                            GlobalOutlineEffectAlpha = minAlphaWhileBlinking;
                            pauseTimer = pauseWhenBlinkOnMinAlpha;
                            blinkForward = true;
                        }
                    }

                    break;
                }

                case States.IDLE:
                    enabled = false;
                    break;

                case States.WORKING:
                    workingStateTimer -= deltaTime;

                    if (enterWorkingState) {
                        if (workingStateTimer <= 0f) {
                            enterWorkingState = false;
                            PlayRadarSplashSound();
                        }
                    } else if (workingFadePause) {
                        if (workingStateTimer <= 0f) {
                            workingFadePause = false;
                            workingStateTimer = workingStateFadeTime;
                            radarFadeSound.SetSoundActive();
                            GlobalWorkingCoeff = 1f;
                        } else {
                            GlobalWorkingCoeff -= 1f / workingStateFadeTime * deltaTime;
                        }
                    } else if (workingStateTimer <= 0f) {
                        PlayRadarSplashSound();
                    } else {
                        float num = (workingStateTimer - workingFadeTimeOffset) / (workingStateFadeTime - workingFadeTimeOffset);
                        GlobalWorkingCoeff = num;
                    }

                    break;
            }
        }

        void OnDestroy() {
            IS_OUTLINE_EFFECT_RUNNING = false;
            GlobalOutlineEffectAlpha = 0f;
        }

        void StopRadarSounds() {
            radarFadeSound.StopImmediately();
            radarSplashSound.StopImmediately();
        }

        void UpdateEffectTransformCenter() {
            Vector3 position = effectCenterTransform.position;
            Shader.SetGlobalVector(worldSpaceEffectCenterID, new Vector4(position.x, position.y, position.z, 1f));
        }

        public void InitializeOutlineEffect(Entity mapEffectEntity, Transform effectCenterTransform) {
            this.mapEffectEntity = mapEffectEntity;
            this.effectCenterTransform = effectCenterTransform;
            radiusSpeed = maxEffectRadius / globalRadiusTime;
            alphaSpeedFadeOut = 1f / globalAlphaFadeOutTime;
            alphaSpeedFadeIn = 1f / globalAlphaFadeInTime;
            globalWorkingCoeffID = Shader.PropertyToID("_WorkingOutlineCoeff");
            globalOutlineEffectAlphaID = Shader.PropertyToID("_GlobalTankOutlineEffectAlpha");
            worldSpaceEffectCenterID = Shader.PropertyToID("_WorldSpaceEffectCenter");
            tankOutlineEffectRadiusID = Shader.PropertyToID("_TankOutlineEffectRadius");
            Shader.SetGlobalFloat("_GlobalTankOutlineScaleMultiplier", blinkRadius);
            State = States.IDLE;
            enabled = false;
        }

        public void ActivateEffect() {
            IS_OUTLINE_EFFECT_RUNNING = true;
            State = States.ACTIVATION;
        }

        public void RunBlinkerForEffect() {
            State = States.BLINKER;
        }

        public void DeactivateEffect() {
            State = States.DEACTIVATION;
        }

        void SwitchEntityState<T>() where T : Node {
            EngineService.Engine.ScheduleEvent(new TankOutlineMapEffectSwitchStateEvent<T>(), mapEffectEntity);
        }

        void PlayRadarSplashSound() {
            StopRadarSounds();
            GlobalWorkingCoeff = 0.375f;
            radarSplashSound.SetSoundActive();
            workingFadePause = true;
            workingStateTimer = radarFadeSoundDelay;
        }

        enum States {
            IDLE = 0,
            ACTIVATION = 1,
            WORKING = 2,
            BLINKER = 3,
            DEACTIVATION = 4
        }
    }
}
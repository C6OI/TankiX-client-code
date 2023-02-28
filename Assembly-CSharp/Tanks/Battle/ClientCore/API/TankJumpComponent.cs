using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1835748384321L)]
    [Shared]
    [SkipExceptionOnAddRemove]
    public class TankJumpComponent : SharedChangeableComponent {
        public static float START_TIME = 0.06f;

        public static float NEAR_START_TIME = 0.2f;

        public static float JUMP_MAX_TIME = 10f;

        public static float SLOWDOWN_TIME = 3f;

        public float StartTime { get; set; }

        public Vector3 Velocity { get; set; }

        public bool OnFly { get; set; }

        public bool Slowdown { get; set; }

        public float SlowdownStartTime { get; set; }

        public void StartJump(Vector3 velocity) {
            StartTime = Time.timeSinceLevelLoad;
            Velocity = velocity;
            OnFly = true;
            Slowdown = false;
            OnChange();
        }

        public void FinishAndSlowdown() {
            if (!Slowdown) {
                Slowdown = true;
                SlowdownStartTime = Time.timeSinceLevelLoad;
            }
        }

        public bool isBegin() => OnFly && Time.timeSinceLevelLoad - StartTime < START_TIME;

        public bool isNearBegin() => OnFly && Time.timeSinceLevelLoad - StartTime < NEAR_START_TIME;

        public bool isFinished() => !OnFly || Time.timeSinceLevelLoad - StartTime > JUMP_MAX_TIME || Slowdown && Time.timeSinceLevelLoad - SlowdownStartTime > SLOWDOWN_TIME;

        public float GetSlowdownLerp() {
            if (isNearBegin()) {
                return 0f;
            }

            if (!Slowdown) {
                return 0f;
            }

            float num = Mathf.Clamp((Time.timeSinceLevelLoad - SlowdownStartTime) / (SLOWDOWN_TIME * 0.8f), 0f, 1f);
            return num * num;
        }
    }
}
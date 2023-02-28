using System;
using System.Collections.Generic;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class SpiderAnimatorComponent : MonoBehaviour, Component {
        public enum SpiderAnimationState {
            DISABLED = 0,
            ACTIVATION = 1,
            IDLE = 2,
            RUN = 3,
            JUMP = 4,
            ATTACK_JUMP = 5
        }

        [SerializeField] bool runiningOnStart;

        [SerializeField] string activationClipName = "Activation";

        [SerializeField] string runClipName = "Walk";

        [SerializeField] float mass = 1f;

        [SerializeField] float runAnimationSpeed = 1f;

        [SerializeField] float runForce = 10f;

        [SerializeField] float rotationSpeed = 10f;

        [SerializeField] float maximalRuningSpeed = 10f;

        [SerializeField] float runingDrag = 1f;

        [SerializeField] string jumpClipName = "Jump";

        [SerializeField] float jumpForce = 500f;

        [SerializeField] float maxDepenetrationVelocity = 2f;

        [SerializeField] string idleClipName = "Idle";

        Animation animation;

        SpiderAnimationState currentState;

        bool jumpForceApplied;

        SpiderAnimationState oldState;

        Rigidbody rigidbody;

        readonly Dictionary<SpiderAnimationState, SpiderAnimationEntry> states = new();

        Rigidbody targetBody;

        public float Speed {
            set => maximalRuningSpeed = value;
        }

        public float Drag {
            set => runingDrag = value;
        }

        public float Acceleration {
            set => runForce = mass * value;
        }

        public bool OnGround { get; private set; }

        void Start() {
            rigidbody = GetComponentInParent<Rigidbody>();
            animation = GetComponentInParent<Animation>();
            rigidbody.mass = mass;
            rigidbody.maxDepenetrationVelocity = maxDepenetrationVelocity;
            ConfigureState(SpiderAnimationState.ACTIVATION, activationClipName, Activation);
            ConfigureState(SpiderAnimationState.IDLE, idleClipName);
            ConfigureState(SpiderAnimationState.RUN, runClipName, Runing);
            ConfigureState(SpiderAnimationState.JUMP, jumpClipName, Jumping);

            if (runiningOnStart) {
                StartRuning();
            }
        }

        void FixedUpdate() {
            AddStabilizeForce();

            if (states.ContainsKey(currentState)) {
                if (oldState != currentState) {
                    animation.CrossFade(states[currentState].ClipName);
                    oldState = currentState;
                    states[currentState].StartTime = Time.timeSinceLevelLoad;
                }

                if (states[currentState].Action != null) {
                    states[currentState].Action();
                }
            }
        }

        public void SetTarget(Rigidbody target) {
            targetBody = target;
        }

        public void StartActivation() {
            currentState = SpiderAnimationState.ACTIVATION;
        }

        public void StartRuning() {
            OnGround = true;
            currentState = SpiderAnimationState.RUN;
        }

        public void StartJump() {
            currentState = SpiderAnimationState.JUMP;
            jumpForceApplied = false;
        }

        public void StartIdle() {
            currentState = SpiderAnimationState.IDLE;
        }

        void ConfigureState(SpiderAnimationState animationState, string clipName, Action action = null) {
            states.Add(animationState, new SpiderAnimationEntry {
                ClipName = clipName,
                Animation = animation[clipName],
                Action = action
            });
        }

        void Activation() {
            SpiderAnimationEntry spiderAnimationEntry = states[currentState];

            if (targetBody != null && spiderAnimationEntry.Animation.normalizedTime % 1f >= 0.99f) {
                StartRuning();
            }
        }

        void Runing() {
            SpiderAnimationEntry spiderAnimationEntry = states[currentState];
            Vector3 directionToTarget = GetDirectionToTarget();

            if (directionToTarget.Equals(Vector3.zero)) {
                spiderAnimationEntry.Animation.speed = 0f;
                return;
            }

            float num = Vector3.Dot(directionToTarget, rigidbody.transform.right);
            float num2 = 0f;

            if (num > 0f) {
                num2 = 1f;
            }

            if (num < 0f) {
                num2 = -1f;
            }

            RaycastHit hitInfo;
            OnGround = Physics.Raycast(transform.position + transform.up * 0.2f, -transform.up, out hitInfo, 1f, LayerMasks.STATIC);

            if (OnGround) {
                float magnitude = rigidbody.velocity.magnitude;

                if (magnitude > maximalRuningSpeed) {
                    rigidbody.SetVelocitySafe(rigidbody.velocity * maximalRuningSpeed / magnitude);
                }

                float magnitude2 = Vector3.ProjectOnPlane(rigidbody.velocity, hitInfo.normal).magnitude;
                spiderAnimationEntry.Animation.speed = magnitude2 * runAnimationSpeed;
                Vector3 vector = directionToTarget;

                if (Vector3.Dot(hitInfo.normal, Vector3.up) > 0.5f) {
                    vector = Vector3.ProjectOnPlane(directionToTarget, hitInfo.normal).normalized;
                }

                vector = (vector + Vector3.up * 0.2f).normalized;
                rigidbody.AddTorqueSafe(0f, num2 * rotationSpeed, 0f);
                rigidbody.AddForceAtPositionSafe(vector * runForce, transform.position);
                rigidbody.drag = runingDrag;
            } else {
                if (Vector3.Dot(rigidbody.transform.up, Vector3.up) < 0.1f) {
                    spiderAnimationEntry.Animation.speed = 4f;
                } else {
                    spiderAnimationEntry.Animation.speed = 0.2f;
                }

                rigidbody.drag = 0f;
            }
        }

        void Jumping() {
            SpiderAnimationEntry spiderAnimationEntry = states[currentState];
            spiderAnimationEntry.Animation.speed = 1.3f;

            if (!jumpForceApplied && spiderAnimationEntry.Animation.normalizedTime % 1f > 0.3f) {
                Vector3 directionToTarget = GetDirectionToTarget();
                Vector3 normalized = (directionToTarget * 0.2f + Vector3.up * 0.8f).normalized;
                rigidbody.AddForceSafe(normalized * jumpForce);
                jumpForceApplied = true;
            }

            if (spiderAnimationEntry.Animation.normalizedTime % 1f > 0.99f) {
                StartRuning();
            }
        }

        Vector3 GetDirectionToTarget() {
            Vector3 result = Vector3.zero;

            if (runiningOnStart) {
                result = Vector3.forward;
            }

            if ((bool)targetBody) {
                result = (targetBody.position - rigidbody.position).normalized;
            }

            return result;
        }

        void AddStabilizeForce() {
            float num = 0.0005f;

            if (Vector3.Dot(rigidbody.transform.up, Vector3.up) < 0.2f) {
                num = 0.2f;
            }

            Quaternion quaternion = Quaternion.FromToRotation(rigidbody.transform.up, Vector3.up);
            rigidbody.AddTorqueSafe(quaternion.eulerAngles * num);
        }

        public class SpiderAnimationEntry {
            public string ClipName { get; set; }

            public AnimationState Animation { get; set; }

            public Action Action { get; set; }

            public float StartTime { get; set; }

            public float TimeSinceStart => Time.timeSinceLevelLoad - StartTime;
        }
    }
}
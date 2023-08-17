using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFrictionCollideSoundBehaviour : MonoBehaviour {
        const float COLLIDE_ANGLE_THRESHOLD = 45f;

        const float MIN_VALUABLE_SOUND_VOLUME = 0.1f;

        const float RAYCAST_EXTRA_LENGTH = 0.25f;

        const int CONTACT_LIMIT_COUNT = 2;

        Vector3 angularVelocity;

        SoundController contactSoundController;

        float halfLength;

        float maxCollisionPower;

        float minCollisionPower;

        Vector3 previousAngularVelocity;

        Vector3 previousVelocity;

        Rigidbody rigidbody;

        Vector3 velocity;

        void Awake() => enabled = false;

        void FixedUpdate() {
            previousVelocity = velocity;
            velocity = rigidbody.velocity;
            previousAngularVelocity = angularVelocity;
            angularVelocity = rigidbody.angularVelocity;
        }

        void OnEnable() {
            velocity = previousVelocity = Vector3.zero;
            previousAngularVelocity = angularVelocity = Vector3.zero;
        }

        void OnCollisionEnter(Collision collision) {
            bool isPlaying = contactSoundController.Source.isPlaying;

            if (isPlaying && contactSoundController.MaxVolume >= 0.1f) {
                return;
            }

            ContactPoint[] contacts = collision.contacts;
            int num = contacts.Length;
            num = num <= 2 ? num : 2;
            Vector3 nrm = Vector3.zero;
            Vector3 nrm2 = Vector3.zero;
            float num2 = 0f;
            bool flag = false;
            bool flag2 = false;
            Vector3 vector = previousAngularVelocity * halfLength;
            Vector3 vector2 = previousVelocity + vector;
            Vector3 to = -vector2.normalized;

            for (int i = 0; i < num; i++) {
                ContactPoint contactPoint = contacts[i];
                Collider otherCollider = contactPoint.otherCollider;
                GameObject gameObject = otherCollider.gameObject;
                int layer = gameObject.layer;
                Vector3 point = contactPoint.point;
                Vector3 position = transform.position;
                Vector3 vector3 = point - position;
                float maxDistance = vector3.magnitude + 0.25f;
                Vector3 normalized = vector3.normalized;
                Ray ray = new(position, normalized);
                RaycastHit hitInfo;

                if (!otherCollider.Raycast(ray, out hitInfo, maxDistance) || hitInfo.collider != otherCollider) {
                    continue;
                }

                Vector3 normal = hitInfo.normal;

                if (layer == Layers.TANK_TO_TANK) {
                    float num3 = Mathf.Abs(Vector3.Angle(normal, to));

                    if (!flag) {
                        flag = true;
                        num2 = num3;
                        nrm2 = normal;
                    } else if (num3 < num2) {
                        num2 = num3;
                        nrm2 = normal;
                    }
                }

                if (flag || layer != Layers.STATIC || gameObject.CompareTag(ClientGraphicsConstants.TERRAIN_TAG)) {
                    continue;
                }

                float num4 = Mathf.Abs(Vector3.Angle(normal, Vector3.up));

                if (!(num4 < 45f)) {
                    float num5 = Mathf.Abs(Vector3.Angle(normal, to));

                    if (!flag2) {
                        flag2 = true;
                        num2 = num5;
                        nrm = normal;
                    } else if (num5 < num2) {
                        num2 = num5;
                        nrm = normal;
                    }
                }
            }

            if (flag) {
                PlayTankCollideSound(vector2, nrm2, isPlaying);
            } else if (flag2) {
                PlayTankCollideSound(vector2, nrm, isPlaying);
            }
        }

        public void Init(SoundController contactSoundController, Rigidbody rigidbody, float halfLength,
            float minCollisionPower, float maxCollisionPower) {
            this.contactSoundController = contactSoundController;
            this.maxCollisionPower = maxCollisionPower;
            this.minCollisionPower = minCollisionPower;
            this.rigidbody = rigidbody;
            this.halfLength = halfLength;
        }

        void PlayTankCollideSound(Vector3 velocity, Vector3 nrm, bool isPlaying) {
            float min = minCollisionPower;
            float max = maxCollisionPower;
            Vector3 vector = velocity - Vector3.ProjectOnPlane(velocity, nrm);
            float num = CalculateVolumeByVelocity(vector, min, max);

            if (num > 0f && (!isPlaying || !(num <= contactSoundController.MaxVolume))) {
                contactSoundController.StopImmediately();
                contactSoundController.MaxVolume = num;
                contactSoundController.SetSoundActive();
            }
        }

        float CalculateVolumeByVelocity(Vector3 velocity, float min, float max) {
            float sqrMagnitude = velocity.sqrMagnitude;
            return Mathf.Clamp01((sqrMagnitude - min) / (max - min));
        }
    }
}
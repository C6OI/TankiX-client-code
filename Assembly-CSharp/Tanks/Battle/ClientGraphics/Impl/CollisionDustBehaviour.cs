using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CollisionDustBehaviour : MonoBehaviour {
        float delay;

        [NonSerialized] public MapDustComponent mapDust;

        public DustEffectBehaviour Effect { get; private set; }

        void Update() {
            delay -= Time.deltaTime;
        }

        void OnCollisionStay(Collision collision) {
            Effect = mapDust.GetEffectByTag(collision.transform, Vector2.zero);

            if (!(Effect == null) && !(delay > 0f)) {
                delay = 1f / Effect.collisionEmissionRate.RandomValue;
                ContactPoint[] contacts = collision.contacts;

                foreach (ContactPoint contactPoint in contacts) {
                    Effect.TryEmitParticle(contactPoint.point, collision.relativeVelocity);
                }
            }
        }
    }
}
using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CollisionDustBehaviour : MonoBehaviour {
        float delay;

        [NonSerialized] public MapDustComponent mapDust;

        public DustEffectBehaviour Effect { get; private set; }

        void Update() => delay -= Time.deltaTime;

        void OnCollisionStay(Collision collision) {
            Effect = mapDust.GetEffectByTag(collision.transform, Vector2.zero);

            if (!(Effect == null) && delay <= 0f) {
                delay = 1f / Effect.collisionEmissionRate.RandomValue;

                for (int i = 0; i < collision.contacts.Length; i++) {
                    Effect.TryEmitParticle(collision.contacts[i].point, collision.relativeVelocity);
                }
            }
        }
    }
}
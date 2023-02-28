using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleCollisionDecal : MonoBehaviour {
    public ParticleSystem DecalParticles;

    public bool IsBilboard;

    public bool InstantiateWhenZeroSpeed;

    public float MaxGroundAngleDeviation = 45f;

    public float MinDistanceBetweenDecals = 0.1f;

    public float MinDistanceBetweenSurface = 0.03f;

    readonly List<GameObject> collidedGameObjects = new();

    readonly List<ParticleCollisionEvent> collisionEvents = new();

    ParticleSystem initiatorPS;

    ParticleSystem.Particle[] particles;

    void OnEnable() {
        collisionEvents.Clear();
        collidedGameObjects.Clear();
        initiatorPS = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[DecalParticles.main.maxParticles];

        if (InstantiateWhenZeroSpeed) {
            InvokeRepeating("CollisionDetect", 0f, 0.1f);
        }
    }

    void OnDisable() {
        if (InstantiateWhenZeroSpeed) {
            CancelInvoke("CollisionDetect");
        }
    }

    void OnParticleCollision(GameObject other) {
        if (InstantiateWhenZeroSpeed) {
            if (!collidedGameObjects.Contains(other)) {
                collidedGameObjects.Add(other);
            }
        } else {
            OnParticleCollisionManual(other);
        }
    }

    void CollisionDetect() {
        int aliveParticles = 0;

        if (InstantiateWhenZeroSpeed) {
            aliveParticles = DecalParticles.GetParticles(particles);
        }

        foreach (GameObject collidedGameObject in collidedGameObjects) {
            OnParticleCollisionManual(collidedGameObject, aliveParticles);
        }
    }

    void OnParticleCollisionManual(GameObject other, int aliveParticles = -1) {
        collisionEvents.Clear();
        int num = initiatorPS.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < num; i++) {
            float num2 = Vector3.Angle(collisionEvents[i].normal, Vector3.up);

            if (num2 > MaxGroundAngleDeviation) {
                continue;
            }

            if (InstantiateWhenZeroSpeed) {
                if (collisionEvents[i].velocity.sqrMagnitude > 0.1f) {
                    continue;
                }

                bool flag = false;

                for (int j = 0; j < aliveParticles; j++) {
                    float num3 = Vector3.Distance(collisionEvents[i].intersection, particles[j].position);

                    if (num3 < MinDistanceBetweenDecals) {
                        flag = true;
                    }
                }

                if (flag) {
                    continue;
                }
            }

            ParticleSystem.EmitParams emitParams = default;
            emitParams.position = collisionEvents[i].intersection + collisionEvents[i].normal * MinDistanceBetweenSurface;
            Vector3 eulerAngles = Quaternion.LookRotation(-collisionEvents[i].normal).eulerAngles;
            eulerAngles.z = Random.Range(0, 360);
            emitParams.rotation3D = eulerAngles;
            DecalParticles.Emit(emitParams, 1);
        }
    }
}
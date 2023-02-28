using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ME_ParticleTrails : MonoBehaviour {
    public GameObject TrailPrefab;

    readonly List<GameObject> currentGO = new();

    Dictionary<uint, GameObject> hashTrails = new();

    readonly Dictionary<uint, GameObject> newHashTrails = new();

    ParticleSystem.Particle[] particles;

    ParticleSystem ps;

    void Start() {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void Update() {
        UpdateTrail();
    }

    void OnEnable() {
        InvokeRepeating("ClearEmptyHashes", 1f, 1f);
    }

    void OnDisable() {
        Clear();
        CancelInvoke("ClearEmptyHashes");
    }

    public void Clear() {
        foreach (GameObject item in currentGO) {
            Destroy(item);
        }

        currentGO.Clear();
    }

    void UpdateTrail() {
        newHashTrails.Clear();
        int num = ps.GetParticles(particles);

        for (int i = 0; i < num; i++) {
            if (!hashTrails.ContainsKey(particles[i].randomSeed)) {
                GameObject gameObject = Instantiate(TrailPrefab, transform.position, default);
                gameObject.transform.parent = transform;
                currentGO.Add(gameObject);
                newHashTrails.Add(particles[i].randomSeed, gameObject);
                gameObject.GetComponent<LineRenderer>().widthMultiplier *= particles[i].startSize;
                continue;
            }

            GameObject gameObject2 = hashTrails[particles[i].randomSeed];

            if (gameObject2 != null) {
                LineRenderer component = gameObject2.GetComponent<LineRenderer>();
                component.startColor *= particles[i].GetCurrentColor(ps);
                component.endColor *= particles[i].GetCurrentColor(ps);

                if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World) {
                    gameObject2.transform.position = particles[i].position;
                }

                if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local) {
                    gameObject2.transform.position = ps.transform.TransformPoint(particles[i].position);
                }

                newHashTrails.Add(particles[i].randomSeed, gameObject2);
            }

            hashTrails.Remove(particles[i].randomSeed);
        }

        foreach (KeyValuePair<uint, GameObject> hashTrail in hashTrails) {
            if (hashTrail.Value != null) {
                hashTrail.Value.GetComponent<ME_TrailRendererNoise>().IsActive = false;
            }
        }

        AddRange(hashTrails, newHashTrails);
    }

    public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection) {
        if (collection == null) {
            return;
        }

        foreach (KeyValuePair<T, S> item in collection) {
            if (!source.ContainsKey(item.Key)) {
                source.Add(item.Key, item.Value);
            }
        }
    }

    void ClearEmptyHashes() {
        hashTrails = hashTrails.Where(h => h.Value != null).ToDictionary(h => h.Key, h => h.Value);
    }
}
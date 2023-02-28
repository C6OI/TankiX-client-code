using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TemperatureEffect : MonoBehaviour {
        [SerializeField] Gradient particlesStartColor;

        [SerializeField] AnimationCurve lightIntensity;

        ParticleSystem particleSystem;

        void Awake() {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public void SetTemperature(float temperature) {
            temperature = Math.Abs(temperature);
            particleSystem.startColor = particlesStartColor.Evaluate(temperature);
        }
    }
}
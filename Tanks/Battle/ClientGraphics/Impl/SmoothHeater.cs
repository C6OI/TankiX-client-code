using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SmoothHeater {
        readonly int burningTimeInMs;
        readonly Material temperatureMaterial;

        readonly MonoBehaviour updater;

        bool burningInProgress;

        bool coolingInProgress;

        float startProcessTime;

        protected float temperature;

        public SmoothHeater(int burningTimeInMs, Material temperatureMaterial, MonoBehaviour updater) {
            this.burningTimeInMs = burningTimeInMs;
            this.temperatureMaterial = temperatureMaterial;
            this.updater = updater;
        }

        public virtual void Heat() {
            burningInProgress = true;
            coolingInProgress = false;
            startProcessTime = Time.time;
            updater.enabled = true;
        }

        public void Cool() {
            coolingInProgress = true;
            burningInProgress = false;
            startProcessTime = Time.time;
            updater.enabled = true;
        }

        public void Update() {
            if (burningInProgress) {
                UpdateBurning();

                if (temperature.Equals(1f)) {
                    FinalizeBurning();
                }
            }

            if (coolingInProgress) {
                UpdateCooling();

                if (temperature.Equals(0f)) {
                    FinalizeCooling();
                }
            }
        }

        protected virtual void UpdateBurning() {
            temperature = Mathf.Clamp01((Time.time - startProcessTime) * 1000f / burningTimeInMs);
            temperatureMaterial.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, temperature);
        }

        protected virtual void UpdateCooling() {
            temperature = 1f - Mathf.Clamp01((Time.time - startProcessTime) * 1000f / burningTimeInMs);
            temperatureMaterial.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, temperature);
        }

        protected void FinalizeBurning() {
            burningInProgress = false;
            updater.enabled = false;
        }

        protected virtual void FinalizeCooling() {
            coolingInProgress = false;
            updater.enabled = false;
        }
    }
}
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SmoothHeaterLighting : SmoothHeater {
        readonly LightContainer lightContainer;

        public SmoothHeaterLighting(int burningTimeInMs, Material temperatureMaterial, MonoBehaviour updater,
            LightContainer lightContainer)
            : base(burningTimeInMs, temperatureMaterial, updater) => this.lightContainer = lightContainer;

        public override void Heat() {
            lightContainer.SetIntensity(0f);
            lightContainer.gameObject.SetActive(true);
            base.Heat();
        }

        protected override void UpdateBurning() {
            base.UpdateBurning();
            UpdateLightsIntencity();
        }

        protected override void UpdateCooling() {
            base.UpdateCooling();
            UpdateLightsIntencity();
        }

        protected override void FinalizeCooling() {
            base.FinalizeCooling();
            lightContainer.gameObject.SetActive(false);
        }

        void UpdateLightsIntencity() => lightContainer.SetIntensity(temperature * lightContainer.maxLightIntensity);
    }
}
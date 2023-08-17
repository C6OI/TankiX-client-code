using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    [RequireComponent(typeof(TemperatureVisualControllerComponent))]
    public class TemperatureEffects : MonoBehaviour, TemperatureChangeListener {
        [SerializeField] GameObject freezingPrefab;

        [SerializeField] GameObject burningPrefab;

        [SerializeField] Transform mountPoint;

        TemperatureEffect burningEffect;

        TemperatureEffect freezingEffect;

        void Awake() {
            burningEffect = InstansiateEffect(burningPrefab);
            freezingEffect = InstansiateEffect(freezingPrefab);
            GetComponent<TemperatureVisualControllerComponent>().listeners.Add(this);
        }

        public void TemperatureChanged(float temperature) {
            UpdateBurningEffect(temperature);
            UpdateFreezingEffect(temperature);
        }

        void UpdateBurningEffect(float temperature) {
            bool flag = temperature > 0f;
            burningEffect.gameObject.SetActive(flag);

            if (flag) {
                burningEffect.SetTemperature(temperature);
            }
        }

        void UpdateFreezingEffect(float temperature) {
            bool flag = temperature < 0f;
            freezingEffect.gameObject.SetActive(flag);

            if (flag) {
                freezingEffect.SetTemperature(temperature);
            }
        }

        TemperatureEffect InstansiateEffect(GameObject prefab) {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.SetParent(mountPoint, false);
            gameObject.SetActive(false);
            return gameObject.GetComponent<TemperatureEffect>();
        }
    }
}
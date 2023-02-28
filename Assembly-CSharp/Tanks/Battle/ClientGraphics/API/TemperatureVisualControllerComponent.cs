using System;
using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class TemperatureVisualControllerComponent : MonoBehaviour, Component {
        public Renderer renderer;

        public List<TemperatureChangeListener> listeners = new();

        float temperature;

        public float Temperature {
            get => temperature;
            set {
                if (!(Math.Abs(temperature - value) < 0.0001)) {
                    temperature = value;

                    if (renderer != null) {
                        TankMaterialsUtil.SetTemperature(renderer, temperature);
                    }

                    int count = listeners.Count;

                    for (int i = 0; i < count; i++) {
                        TemperatureChangeListener temperatureChangeListener = listeners[i];
                        temperatureChangeListener.TemperatureChanged(temperature);
                    }
                }
            }
        }

        public void Reset() {
            temperature = 0f;

            if (renderer != null) {
                TankMaterialsUtil.SetTemperature(renderer, temperature);
            }

            int count = listeners.Count;

            for (int i = 0; i < count; i++) {
                TemperatureChangeListener temperatureChangeListener = listeners[i];
                temperatureChangeListener.TemperatureChanged(temperature);
            }
        }
    }
}
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
                if (Math.Abs(temperature - value) < 0.0001) {
                    return;
                }

                temperature = value;

                if (renderer != null) {
                    TankMaterialsUtil.SetTemperature(renderer, temperature);
                }

                foreach (TemperatureChangeListener listener in listeners) {
                    listener.TemperatureChanged(temperature);
                }
            }
        }
    }
}
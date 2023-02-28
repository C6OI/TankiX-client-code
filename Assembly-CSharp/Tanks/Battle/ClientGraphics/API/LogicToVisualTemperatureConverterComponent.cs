using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class LogicToVisualTemperatureConverterComponent : MonoBehaviour, Component {
        [SerializeField] AnimationCurve logicTemperatureToVisualTemperature;

        public float ConvertToVisualTemperature(float logicTemperature) => logicTemperatureToVisualTemperature.Evaluate(logicTemperature);
    }
}
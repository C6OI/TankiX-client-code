using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class PelletDirectionsCalculator : Component {
        public static Vector3[] GetRandomDirections(HammerPelletConeComponent config, Quaternion worldRotation,
            Vector3 localDirection, int seed) {
            int num = Mathf.FloorToInt(config.PelletCount);
            Vector3[] array = new Vector3[num];
            int seed2 = Random.seed;
            Random.seed = seed;

            for (int i = 0; i < num; i++) {
                Vector3 euler = Random.insideUnitCircle;
                euler.x *= config.HorizontalConeHalfAngle;
                euler.y *= config.VerticalConeHalfAngle;
                array[i] = worldRotation * Quaternion.Euler(euler) * localDirection;
            }

            Random.seed = seed2;
            return array;
        }
    }
}
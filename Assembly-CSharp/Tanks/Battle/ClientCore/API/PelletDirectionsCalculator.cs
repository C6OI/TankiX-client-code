using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public static class PelletDirectionsCalculator {
        public static Vector3[] GetRandomDirections(HammerPelletConeComponent config, Quaternion worldRotation, Vector3 localDirection) {
            int num = Mathf.FloorToInt(config.PelletCount);
            Vector3[] array = new Vector3[num];
            int seed = Random.seed;
            Random.seed = config.ShotSeed;

            for (int i = 0; i < num; i++) {
                Vector3 euler = Random.insideUnitCircle;
                euler.x *= config.HorizontalConeHalfAngle;
                euler.y *= config.VerticalConeHalfAngle;
                array[i] = worldRotation * Quaternion.Euler(euler) * localDirection;
            }

            Random.seed = seed;
            return array;
        }
    }
}
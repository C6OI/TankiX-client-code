using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class PathData {
        public Vector3[] currentPath;

        public int currentPathIndex;
        public float timeToRecalculatePath;
    }
}
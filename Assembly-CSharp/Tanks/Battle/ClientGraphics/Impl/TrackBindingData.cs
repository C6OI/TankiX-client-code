using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [Serializable]
    public class TrackBindingData {
        public Transform[] trackPointsJoints;

        public Vector3[] trackPointsPositions;

        public Transform[] movingWheelsJoints;

        public float[] movingWheelsRadiuses;

        public int[] movingWheelsNearestTrackPointsIndices;

        public Transform[] rotatingWheelsJoints;

        public float[] rotatingWheelsRadiuses;

        public int[] trackSegmentsIndices;

        public float centerX;
    }
}
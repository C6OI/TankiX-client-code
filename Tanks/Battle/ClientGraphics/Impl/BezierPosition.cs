using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BezierPosition {
        const float MAX_OFFSET = 0.1f;

        float baseRatio = 0.05f;

        public Vector2 cameraPosition;

        public float distanceToPivot;

        public float elevationAngle;

        float offset;

        public Vector2 point0 = new(1.45f, 5.45f);

        public Vector2 point1 = new(9.3f, 13.95f);

        public Vector2 point2 = new(22.45f, 15.65f);

        public Vector2 point3 = new(31.05f, 7.6f);

        public BezierPosition() => Apply();

        float ratio => Mathf.Clamp01(baseRatio + offset);

        public float GetBaseRatio() => baseRatio;

        public void SetBaseRatio(float value) {
            baseRatio = Mathf.Clamp01(value);
            Apply();
        }

        public float GetRatioOffset() => (offset + 0.05f) / 0.1f;

        public void SetRatioOffset(float value) {
            offset = Mathf.Lerp(-0.05f, 0.05f, Mathf.Clamp01(value));
            Apply();
        }

        public void Apply() {
            cameraPosition = Bezier.PointOnCurve(ratio, point0, point1, point2, point3);
            elevationAngle = Mathf.Atan2(cameraPosition.x, cameraPosition.y);
            distanceToPivot = cameraPosition.magnitude;
        }

        public float GetDistanceToPivot() => distanceToPivot;

        public float GetCameraHeight() => cameraPosition.x;

        public float GetCameraHorizontalDistance() => cameraPosition.y;
    }
}
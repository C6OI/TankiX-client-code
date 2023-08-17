using System;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackController {
        const float additionalAnimationPenetrationBias = 0.02f;

        public float centerX;

        RaycastHit fromHit;

        public float highDistance;

        RaycastHit hit;

        public float lowDistance;

        public MovingWheel[] movingWheels;

        public Wheel[] rotatingWheels;

        public TrackSegment[] segments;

        float slidePosition;

        RaycastHit toHit;

        public TrackPoint[] trackPoints;

        public void UpdateWheelsRotation(float offset) {
            slidePosition = offset;

            for (int i = 0; i < rotatingWheels.Length; i++) {
                Wheel wheel = rotatingWheels[i];

                if (wheel.radius > 0f) {
                    wheel.SetRotation(slidePosition / wheel.radius / ((float)Math.PI / 180f));
                }
            }

            for (int j = 0; j < movingWheels.Length; j++) {
                Wheel wheel2 = movingWheels[j];

                if (wheel2.radius > 0f) {
                    wheel2.SetRotation(slidePosition / wheel2.radius / ((float)Math.PI / 180f));
                }
            }
        }

        public void AnimateWithAdditionalRays(Transform transform, float smoothSpeed, float maxStretchingCoeff) {
            AnimateTrackPointsPositions(transform);
            AnimateTrackPointsWithAdditionalRays(transform);
            CorrectTrackSegmentsStretching(maxStretchingCoeff);
            UpdateJointsPositions(transform, smoothSpeed);
        }

        public void Animate(Transform transform, float smoothSpeed, float maxStretchingCoeff) {
            AnimateTrackPointsPositions(transform);
            CorrectTrackSegmentsStretching(maxStretchingCoeff);
            UpdateJointsPositions(transform, smoothSpeed);
        }

        void UpdateJointsPositions(Transform transform, float smoothSpeed) {
            float num = Time.deltaTime * smoothSpeed;

            for (int i = 0; i < trackPoints.Length; i++) {
                TrackPoint trackPoint = trackPoints[i];
                float num2 = trackPoint.desiredVerticalPosition - trackPoint.currentPosition.y;
                num2 = !(num2 > 0f) ? !(0f - num2 > num) ? num2 : 0f - num : !(num2 > num) ? num2 : num;
                trackPoint.currentPosition.y += num2;
            }

            for (int j = 0; j < trackPoints.Length; j++) {
                TrackPoint trackPoint2 = trackPoints[j];
                trackPoint2.UpdateTrackPointPosition(transform, trackPoint2.currentPosition);
            }

            for (int k = 0; k < movingWheels.Length; k++) {
                MovingWheel movingWheel = movingWheels[k];
                movingWheel.UpdateObjPosition(transform, movingWheel.nearestPoint.currentPosition);
            }
        }

        void AnimateTrackPointsPositions(Transform transform) {
            float gravityCorrection = GetGravityCorrection(transform);

            for (int i = 0; i < trackPoints.Length; i++) {
                TrackPoint trackPoint = trackPoints[i];

                Vector3 vector =
                    transform.TransformPoint(GetPointWithVerticalOffset(trackPoint.initialPosition, highDistance));

                Vector3 vector2 = transform.TransformPoint(GetPointWithVerticalOffset(trackPoint.initialPosition,
                    (0f - lowDistance) * gravityCorrection));

                Vector3 direction = vector2 - vector;
                float magnitude = direction.magnitude;

                if (Physics.Raycast(vector, direction, out hit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION)) {
                    trackPoint.desiredVerticalPosition = trackPoint.initialPosition.y +
                                                         Mathf.Lerp(highDistance,
                                                             0f - lowDistance,
                                                             hit.distance / magnitude);
                } else {
                    trackPoint.desiredVerticalPosition = trackPoint.initialPosition.y - lowDistance * gravityCorrection;
                }
            }
        }

        void AnimateTrackPointsWithAdditionalRays(Transform transform) {
            for (int i = 0; i < segments.Length; i++) {
                TrackSegment trackSegment = segments[i];
                Vector3 initialPosition = trackSegment.a.initialPosition;
                Vector3 initialPosition2 = trackSegment.b.initialPosition;

                Vector3 vector = transform.TransformPoint(new Vector3(initialPosition.x,
                    trackSegment.a.desiredVerticalPosition + 0.02f,
                    initialPosition.z));

                Vector3 vector2 = transform.TransformPoint(new Vector3(initialPosition2.x,
                    trackSegment.b.desiredVerticalPosition + 0.02f,
                    initialPosition2.z));

                Vector3 vector3 = vector2 - vector;
                float magnitude = vector3.magnitude;

                if (Physics.Raycast(vector, vector3, out fromHit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION)) {
                    vector3 = -vector3;

                    Vector3 vector4 =
                        !Physics.Raycast(vector2, vector3, out toHit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION)
                            ? fromHit.point : Vector3.Lerp(fromHit.point, toHit.point, 0.5f);

                    float num = (vector4 - vector).magnitude / magnitude;
                    Vector3 position = Vector3.Lerp(initialPosition, initialPosition2, num);
                    position.y = (1f - num) * initialPosition.y + num * initialPosition2.y + highDistance;
                    position = transform.TransformPoint(position);
                    vector3 = vector4 - position;
                    magnitude = vector3.magnitude;

                    if (Physics.Raycast(position, vector3, out hit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION)) {
                        float num2 = transform.InverseTransformPoint(hit.point).y -
                                     (trackSegment.a.desiredVerticalPosition * (1f - num) +
                                      trackSegment.b.desiredVerticalPosition * num);

                        float num3 = num2 / (1f - num - num + num * num + num * num);
                        trackSegment.a.desiredVerticalPosition += (1f - num) * num3;
                        trackSegment.b.desiredVerticalPosition += num * num3;
                    }
                }
            }
        }

        float GetGravityCorrection(Transform transform) {
            Vector3 vector = transform.InverseTransformDirection(Vector3.up);
            float num = vector.y / vector.magnitude;
            return Mathf.Clamp01(num / 0.3f);
        }

        void CorrectTrackSegmentsStretching(float maxStretchingCoeff) {
            for (int i = 0; i < segments.Length; i++) {
                TrackSegment trackSegment = segments[i];
                TrackPoint a = trackSegment.a;
                TrackPoint b = trackSegment.b;

                float num = Vector3.Distance(GetPointWithVerticalPosition(a.initialPosition, a.desiredVerticalPosition),
                    GetPointWithVerticalPosition(b.initialPosition, b.desiredVerticalPosition));

                float num2 = num / trackSegment.length;

                if (num2 > maxStretchingCoeff) {
                    float num3 = trackSegment.length * maxStretchingCoeff;
                    float num4 = a.initialPosition.x - b.initialPosition.x;
                    float num5 = a.desiredVerticalPosition - b.desiredVerticalPosition;
                    float num6 = a.initialPosition.z - b.initialPosition.z;

                    if (num5 < 0f) {
                        num5 = Mathf.Sqrt(num3 * num3 - num4 * num4 - num6 * num6);
                        a.desiredVerticalPosition = b.desiredVerticalPosition - num5;
                    } else {
                        num5 = Mathf.Sqrt(num3 * num3 - num4 * num4 - num6 * num6);
                        b.desiredVerticalPosition = a.desiredVerticalPosition - num5;
                    }
                }
            }
        }

        static Vector3 GetPointWithVerticalOffset(Vector3 initialPoint, float verticalOffset) =>
            new(initialPoint.x, initialPoint.y + verticalOffset, initialPoint.z);

        static Vector3 GetPointWithVerticalPosition(Vector3 point, float verticalPosition) =>
            new(point.x, verticalPosition, point.z);
    }
}
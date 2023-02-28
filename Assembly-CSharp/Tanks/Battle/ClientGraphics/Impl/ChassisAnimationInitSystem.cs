using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ChassisAnimationInitSystem : ECSSystem {
        [OnEventFire]
        public void OnInit(ChassisInitEvent e, ChassisAnimationInitNode node) {
            Transform transform = node.tankVisualRoot.transform;
            ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
            ChassisTrackControllerComponent chassisTrackControllerComponent = new();

            if (chassisTrackControllerComponent.LeftTrack == null) {
                chassisTrackControllerComponent.LeftTrack = ConstructController(chassisAnimation.leftTrackData, transform);
            }

            if (chassisTrackControllerComponent.RightTrack == null) {
                chassisTrackControllerComponent.RightTrack = ConstructController(chassisAnimation.rightTrackData, transform);
            }

            chassisAnimation.TracksMaterial = TankMaterialsUtil.GetTrackMaterial(node.trackRenderer.Renderer);
            node.Entity.AddComponent(chassisTrackControllerComponent);
        }

        TrackController ConstructController(TrackBindingData data, Transform root) {
            TrackController trackController = new();
            TrackPoint[] array = new TrackPoint[data.trackPointsJoints.Length];

            for (int i = 0; i < data.trackPointsJoints.Length; i++) {
                Transform transform = data.trackPointsJoints[i];
                Vector3 vector = data.trackPointsPositions[i];
                Vector3 trackPointOffset = root.InverseTransformPoint(transform.position) - vector;
                array[i] = new TrackPoint(vector, transform, trackPointOffset);
            }

            trackController.trackPoints = array;
            MovingWheel[] array2 = new MovingWheel[data.movingWheelsJoints.Length];

            for (int j = 0; j < data.movingWheelsJoints.Length; j++) {
                Transform transform2 = data.movingWheelsJoints[j];
                int num = data.movingWheelsNearestTrackPointsIndices[j];
                MovingWheel movingWheel;

                if (num >= 0) {
                    Vector3 vector2 = data.trackPointsPositions[num];
                    Vector3 offset = root.InverseTransformPoint(transform2.position) - vector2;
                    movingWheel = new MovingWheel(transform2, array[num], offset);
                } else {
                    movingWheel = new MovingWheel(transform2);
                }

                if (data.movingWheelsRadiuses != null) {
                    movingWheel.radius = data.movingWheelsRadiuses[j];
                }

                array2[j] = movingWheel;
            }

            trackController.movingWheels = array2;
            Wheel[] array3 = new Wheel[data.rotatingWheelsJoints.Length];

            for (int k = 0; k < data.rotatingWheelsJoints.Length; k++) {
                Wheel wheel = new(data.rotatingWheelsJoints[k]);

                if (data.rotatingWheelsRadiuses != null) {
                    wheel.radius = data.rotatingWheelsRadiuses[k];
                }

                array3[k] = wheel;
            }

            trackController.rotatingWheels = array3;

            if (data.trackPointsJoints.Length > 1) {
                TrackSegment[] array4 = new TrackSegment[data.trackPointsJoints.Length - 1];

                for (int l = 1; l < data.trackPointsJoints.Length; l++) {
                    TrackPoint a = array[l - 1];
                    TrackPoint b = array[l];
                    Vector3 vector3 = data.trackPointsPositions[l - 1];
                    Vector3 vector4 = data.trackPointsPositions[l];
                    array4[l - 1] = new TrackSegment(a, b, (vector4 - vector3).magnitude);
                }

                trackController.segments = array4;
            } else {
                trackController.segments = new TrackSegment[0];
            }

            trackController.centerX = data.centerX;
            return trackController;
        }

        public class ChassisAnimationInitNode : Node {
            public ChassisAnimationComponent chassisAnimation;
            public TankVisualRootComponent tankVisualRoot;

            public TrackRendererComponent trackRenderer;
        }
    }
}
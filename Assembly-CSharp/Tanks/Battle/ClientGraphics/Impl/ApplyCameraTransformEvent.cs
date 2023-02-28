using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ApplyCameraTransformEvent : Event {
        static readonly ApplyCameraTransformEvent INSTANCE = new();

        float deltaTime;

        float positionSmoothingRatio;

        float rotationSmoothingRatio;

        ApplyCameraTransformEvent() {
            ResetFields();
        }

        public float PositionSmoothingRatio {
            get => positionSmoothingRatio;
            set {
                PositionSmoothingRatioValid = true;
                positionSmoothingRatio = value;
            }
        }

        public float RotationSmoothingRatio {
            get => rotationSmoothingRatio;
            set {
                RotationSmoothingRatioValid = true;
                rotationSmoothingRatio = value;
            }
        }

        public float DeltaTime {
            get => deltaTime;
            set {
                DeltaTimeValid = true;
                deltaTime = value;
            }
        }

        public bool PositionSmoothingRatioValid { get; private set; }

        public bool RotationSmoothingRatioValid { get; private set; }

        public bool DeltaTimeValid { get; private set; }

        void ResetFields() {
            PositionSmoothingRatioValid = false;
            RotationSmoothingRatioValid = false;
            DeltaTimeValid = false;
        }

        public static ApplyCameraTransformEvent ResetApplyCameraTransformEvent() {
            INSTANCE.ResetFields();
            return INSTANCE;
        }
    }
}
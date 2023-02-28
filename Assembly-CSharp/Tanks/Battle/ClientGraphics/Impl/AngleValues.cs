namespace Tanks.Battle.ClientGraphics.Impl {
    public class AngleValues {
        static readonly float PI = 180f;

        float angleDirection;

        float angularAcceleration;

        float angularSpeed;

        float currentAngle;

        float totalAngle;

        public AngleValues(float startAngle, float targetAngle, float accelerationCoeff) {
            CalculateAngleAndDirection(startAngle, targetAngle);
            CalculateShortestAngle();
            CalculateAccelerationAndSpeed(accelerationCoeff);
        }

        void CalculateAngleAndDirection(float startAngle, float targetAngle) {
            totalAngle = targetAngle - startAngle;

            if (totalAngle < 0f) {
                totalAngle = 0f - totalAngle;
                angleDirection = -1f;
            } else {
                angleDirection = 1f;
            }
        }

        void CalculateShortestAngle() {
            if (totalAngle > PI) {
                angleDirection = 0f - angleDirection;
                totalAngle = 2f * PI - totalAngle;
            }
        }

        void CalculateAccelerationAndSpeed(float accelerationCoeff) {
            angularAcceleration = accelerationCoeff * totalAngle;
            angularSpeed = 0f;
            currentAngle = 0f;
        }

        public void ReverseAcceleration() {
            angularAcceleration = 0f - angularAcceleration;
        }

        public float Update(float dt) {
            if (currentAngle < totalAngle) {
                float num = angularAcceleration * dt;
                float num2 = (angularSpeed + 0.5f * num) * dt;
                angularSpeed += num;
                float num3 = totalAngle - currentAngle;

                if (num3 < num2) {
                    num2 = num3;
                }

                currentAngle += num2;
                return num2 * angleDirection;
            }

            return 0f;
        }
    }
}
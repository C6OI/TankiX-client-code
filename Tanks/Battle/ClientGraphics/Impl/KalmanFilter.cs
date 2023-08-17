using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class KalmanFilter {
        public static readonly float MEASUREMENT_NOISE = 2f;

        public static readonly float ENVIRONMENT_NOISE = 20f;

        public static readonly float FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE = 1f;

        public static readonly float FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE = 1f;

        public static readonly float INIT_COVARIANCE = 0.1f;

        float covariance;

        float predictedCovariance;

        Vector3 predictedState;

        public KalmanFilter(Vector3 initState) => Reset(initState);

        public Vector3 State { get; private set; }

        public void Reset(Vector3 initState) {
            State = initState;
            covariance = INIT_COVARIANCE;
        }

        public void Correct(Vector3 data) {
            TimeUpdatePrediction();
            MeasurementUpdateCorrection(data);
        }

        void TimeUpdatePrediction() {
            predictedState = FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE * State;

            predictedCovariance = FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE * covariance * FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE +
                                  MEASUREMENT_NOISE;
        }

        void MeasurementUpdateCorrection(Vector3 data) {
            float num = FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE *
                        predictedCovariance /
                        (FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE *
                         predictedCovariance *
                         FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE +
                         ENVIRONMENT_NOISE);

            State = predictedState + num * (data - FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE * predictedState);
            covariance = (1f - num * FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE) * predictedCovariance;
        }
    }
}
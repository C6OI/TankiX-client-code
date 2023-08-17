using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoteTankSmootherSystem : ECSSystem {
        [OnEventFire]
        public void OnTankCreation(NodeAddedEvent e, RemoteTankNode node) {
            Entity entity = node.Entity;
            KalmanFilterComponent kalmanFilterComponent = new();
            Transform transform = node.rigidbody.Rigidbody.transform;
            kalmanFilterComponent.kalmanFilterPosition = new KalmanFilter(transform.position);
            entity.AddComponent(kalmanFilterComponent);
            entity.AddComponent<RemoteTankSmootherComponent>();
            ScheduleEvent<PositionSmoothingSnapEvent>(node);
        }

        [OnEventFire]
        public void InitTimeSmoother(NodeAddedEvent e, RemoteSmoothTankNode node) =>
            node.Entity.AddComponent(new TransformTimeSmoothingComponent(node.tankVisualRoot.transform));

        [OnEventFire]
        public void SnapOnMovementInit(TankMovementInitEvent e, RemoteTankNode tank) =>
            ScheduleEvent<PositionSmoothingSnapEvent>(tank);

        [OnEventFire]
        public void OnPositionSnap(PositionSmoothingSnapEvent e, RemoteSmoothTankNode node) {
            Transform transform = node.rigidbody.Rigidbody.transform;
            node.remoteTankSmoother.prevVisualPosition = transform.position;
            node.remoteTankSmoother.prevVisualRotation = transform.rotation;
            Transform transform2 = node.tankVisualRoot.transform;
            transform2.position = transform.position;
            transform2.rotation = transform.rotation;
            node.kalmanFilter.kalmanFilterPosition.Reset(transform.position);
        }

        [OnEventFire]
        public void OnLocalTankDestruction(NodeRemoveEvent e, RemoteTankNode node) {
            Entity entity = node.Entity;
            entity.RemoveComponent<KalmanFilterComponent>();
            entity.RemoveComponent<RemoteTankSmootherComponent>();
        }

        void KalmanFPSIndependentCorrect(KalmanFilterComponent kalmanFilterComponent, Vector3 tankPosition, float dt) {
            kalmanFilterComponent.kalmanUpdateTimeAccumulator += dt;

            while (kalmanFilterComponent.kalmanUpdateTimeAccumulator > kalmanFilterComponent.kalmanUpdatePeriod) {
                kalmanFilterComponent.kalmanFilterPosition.Correct(tankPosition);
                kalmanFilterComponent.kalmanUpdateTimeAccumulator -= kalmanFilterComponent.kalmanUpdatePeriod;
            }
        }

        [OnEventFire]
        public void OnUpdate(TimeUpdateEvent e, RemoteSmoothTankNode node) {
            float deltaTime = e.DeltaTime;
            KalmanFilterComponent kalmanFilter = node.kalmanFilter;
            RemoteTankSmootherComponent remoteTankSmoother = node.remoteTankSmoother;
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            Transform transform = node.tankVisualRoot.transform;
            Transform transform2 = node.rigidbody.Rigidbody.transform;
            KalmanFPSIndependentCorrect(kalmanFilter, transform2.position, deltaTime);
            float smoothingCoeff = deltaTime * remoteTankSmoother.smoothingCoeff;

            remoteTankSmoother.prevVisualPosition = SmoothPositionValue(remoteTankSmoother.prevVisualPosition,
                rigidbody.velocity,
                deltaTime,
                kalmanFilter.kalmanFilterPosition.State,
                smoothingCoeff);

            remoteTankSmoother.prevVisualRotation = SmoothRotationValue(remoteTankSmoother.prevVisualRotation,
                rigidbody.angularVelocity,
                deltaTime,
                transform2.rotation,
                smoothingCoeff);

            transform.position = remoteTankSmoother.prevVisualPosition;
            transform.rotation = remoteTankSmoother.prevVisualRotation;
        }

        Vector3 SmoothPositionValue(Vector3 currentValue, Vector3 changeSpeed, float dt, Vector3 targetValue,
            float smoothingCoeff) {
            currentValue += changeSpeed * dt;
            return Vector3.Lerp(currentValue, targetValue, smoothingCoeff);
        }

        Quaternion SmoothRotationValue(Quaternion currentValue, Vector3 changeSpeed, float dt, Quaternion targetValue,
            float smoothingCoeff) {
            currentValue *= Quaternion.Euler(changeSpeed * dt * 57.29578f);
            return Quaternion.Slerp(currentValue, targetValue, smoothingCoeff);
        }

        public class RemoteTankNode : Node {
            public RemoteTankComponent remoteTank;

            public RigidbodyComponent rigidbody;
        }

        public class RemoteSmoothTankNode : Node {
            public KalmanFilterComponent kalmanFilter;
            public RemoteTankComponent remoteTank;

            public RemoteTankSmootherComponent remoteTankSmoother;

            public RigidbodyComponent rigidbody;

            public TankVisualRootComponent tankVisualRoot;
        }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseEjectionSystem : ECSSystem {
        const float MAX_WORK_DISTANCE = 30f;

        [OnEventFire]
        public void CreateCaseContainer(NodeAddedEvent e, SingleNode<MapInstanceComponent> node) {
            node.Entity.AddComponent(new CartridgeCaseContainerComponent());
        }

        [OnEventFire]
        public void RemoveCaseContainer(NodeRemoveEvent e, SingleNode<MapInstanceComponent> node) {
            node.Entity.RemoveComponent<CartridgeCaseContainerComponent>();
        }

        [OnEventFire]
        public void SetupEjectionTrigger(NodeAddedEvent e, SingleNode<CartridgeCaseEjectionTriggerComponent> node) {
            node.component.Entity = node.Entity;
        }

        [OnEventFire]
        public void EjectCase(CartridgeCaseEjectionEvent e, SingleNode<CartridgeCaseEjectorComponent> ejectorNode, [JoinByTank] HullNode hullNode,
            [JoinAll] SingleNode<CartridgeCaseContainerComponent> containerNode) {
            if (hullNode.Entity.HasComponent<SelfTankComponent>() || hullNode.cameraVisibleTrigger.IsVisibleAtRange(30f)) {
                GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
                getInstanceFromPoolEvent.Prefab = ejectorNode.component.casePrefab;
                GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
                ScheduleEvent(getInstanceFromPoolEvent2, ejectorNode);
                GameObject gameObject = getInstanceFromPoolEvent2.Instance.gameObject;
                SetCaseTransform(gameObject, ejectorNode.component);
                SetCaseVelocity(gameObject, ejectorNode.component, hullNode);
                gameObject.SetActive(true);
            }
        }

        void SetCaseTransform(GameObject cartridgeCase, CartridgeCaseEjectorComponent component) {
            cartridgeCase.transform.position = component.transform.TransformPoint(Vector3.zero);
            cartridgeCase.transform.Rotate(component.transform.eulerAngles);
        }

        void SetCaseVelocity(GameObject cartridgeCase, CartridgeCaseEjectorComponent component, HullNode hullNode) {
            GameObject hullInstance = hullNode.hullInstance.HullInstance;
            Rigidbody rigidbody = hullNode.rigidbody.Rigidbody;
            Rigidbody component2 = cartridgeCase.GetComponent<Rigidbody>();
            Vector3 vector = component.transform.TransformDirection(component.initialSpeed * Vector3.forward);
            Vector3 vector2 = component.transform.TransformDirection(component.initialAngularSpeed * Vector3.up);
            Vector3 rhs = cartridgeCase.transform.position - hullInstance.transform.position;
            component2.SetVelocitySafe(vector + rigidbody.velocity + Vector3.Cross(rigidbody.angularVelocity, rhs));
            component2.SetAngularVelocitySafe(vector2 + rigidbody.angularVelocity);
        }

        public class HullNode : Node {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;

            public HullInstanceComponent hullInstance;

            public RigidbodyComponent rigidbody;
        }
    }
}
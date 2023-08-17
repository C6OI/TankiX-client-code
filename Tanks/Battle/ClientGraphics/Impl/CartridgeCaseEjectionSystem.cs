using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseEjectionSystem : ECSSystem {
        const float MAX_WORK_DISTANCE = 30f;

        const string CONTAINER_OBJECT_NAME = "CartridgeCaseContainer";

        [OnEventFire]
        public void CreateCaseContainer(NodeAddedEvent e, SingleNode<MapInstanceComponent> node) =>
            node.Entity.AddComponent(new CartridgeCaseContainerComponent(new GameObject("CartridgeCaseContainer")));

        [OnEventFire]
        public void DestroyCaseContainer(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map,
            [JoinSelf] ContainerNode node) {
            node.Entity.RemoveComponent<CartridgeCaseContainerComponent>();
            Object.Destroy(node.cartridgeCaseContainer.container);
        }

        [OnEventFire]
        public void OnMapEntityDeleted(NodeRemoveEvent e, SingleNode<CartridgeCaseContainerComponent> node) =>
            Object.Destroy(node.component.container);

        [OnEventFire]
        public void SetupEjectionTrigger(NodeAddedEvent e, SingleNode<CartridgeCaseEjectionTriggerComponent> node) =>
            node.component.Entity = node.Entity;

        [OnEventFire]
        public void EjectCase(CartridgeCaseEjectionEvent e, SingleNode<CartridgeCaseEjectorComponent> ejectorNode,
            [JoinByTank] HullNode hullNode, [JoinAll] SingleNode<CartridgeCaseContainerComponent> containerNode) {
            if (hullNode.cameraVisibleTrigger.IsVisibleAtRange(30f)) {
                GameObject gameObject = Object.Instantiate(ejectorNode.component.casePrefab);
                SetCaseTransform(gameObject, ejectorNode.component);
                SetCaseVelocity(gameObject, ejectorNode.component, hullNode);
                gameObject.transform.parent = containerNode.component.container.transform;
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
            component2.velocity = vector + rigidbody.velocity + Vector3.Cross(rigidbody.angularVelocity, rhs);
            component2.angularVelocity = vector2 + rigidbody.angularVelocity;
        }

        public class ContainerNode : Node {
            public CartridgeCaseContainerComponent cartridgeCaseContainer;

            public MapInstanceComponent mapInstance;
        }

        public class HullNode : Node {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;

            public HullInstanceComponent hullInstance;

            public RigidbodyComponent rigidbody;
        }
    }
}
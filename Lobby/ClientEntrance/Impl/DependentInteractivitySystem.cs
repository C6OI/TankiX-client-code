using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientEntrance.Impl {
    public class DependentInteractivitySystem : ECSSystem {
        [OnEventFire]
        public void InitESM(NodeAddedEvent e, [Combine] InteractivityPrerequisiteNode prerequisite,
            [Context] [JoinByScreen] DependentInteractivityNode interactableElement) {
            InteractivityPrerequisiteESMComponent interactivityPrerequisiteESMComponent = new();
            prerequisite.Entity.AddComponent(interactivityPrerequisiteESMComponent);
            EntityStateMachine esm = interactivityPrerequisiteESMComponent.Esm;
            esm.AddState<InteractivityPrerequisiteStates.AcceptableState>();
            esm.AddState<InteractivityPrerequisiteStates.NotAcceptableState>();
            esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
        }

        [OnEventFire]
        public void HandleNotAcceptableState(NodeAddedEvent e,
            [Combine] InteractivityPrerequisiteStates.NotAcceptableState prerequisite,
            [JoinByScreen] [Context] DependentInteractivityNode interactableElement) =>
            interactableElement.dependentInteractivity.SetInteractable(false);

        [OnEventFire]
        public void HandleAcceptableState(NodeAddedEvent e,
            [Combine] InteractivityPrerequisiteStates.AcceptableState prerequisiteNode,
            [Context] [JoinByScreen] DependentInteractivityNode interactableElementNode) {
            DependentInteractivityComponent dependentInteractivity = interactableElementNode.dependentInteractivity;

            for (int i = 0; i < dependentInteractivity.prerequisitesObjects.Count; i++) {
                GameObject gameObject = dependentInteractivity.prerequisitesObjects[i].gameObject;

                if (gameObject.activeInHierarchy &&
                    gameObject.GetComponent<EntityBehaviour>().Entity.HasComponent<NotAcceptableStateComponent>()) {
                    dependentInteractivity.SetInteractable(false);
                    return;
                }
            }

            dependentInteractivity.SetInteractable(true);
        }

        public class InteractivityPrerequisiteNode : Node {
            public InteractivityPrerequisiteComponent interactivityPrerequisite;

            public ScreenGroupComponent screenGroup;
        }

        public class DependentInteractivityNode : Node {
            public DependentInteractivityComponent dependentInteractivity;

            public ScreenGroupComponent screenGroup;
        }
    }
}
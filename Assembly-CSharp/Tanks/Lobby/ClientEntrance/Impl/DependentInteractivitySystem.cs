using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class DependentInteractivitySystem : ECSSystem {
        [OnEventFire]
        public void InitESM(NodeAddedEvent e, [Combine] InteractivityPrerequisiteNode prerequisite, [JoinByScreen] [Combine] [Context] DependentInteractivityNode interactableElement) {
            if (!prerequisite.Entity.HasComponent<InteractivityPrerequisiteESMComponent>()) {
                InteractivityPrerequisiteESMComponent interactivityPrerequisiteESMComponent = new();
                prerequisite.Entity.AddComponent(interactivityPrerequisiteESMComponent);
                EntityStateMachine esm = interactivityPrerequisiteESMComponent.Esm;
                esm.AddState<InteractivityPrerequisiteStates.AcceptableState>();
                esm.AddState<InteractivityPrerequisiteStates.NotAcceptableState>();
                esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        [OnEventFire]
        public void HandleNotAcceptableState(NodeAddedEvent e, [Combine] InteractivityPrerequisiteStates.NotAcceptableState prerequisite,
            [JoinByScreen] [Combine] [Context] DependentInteractivityNode interactableElement) {
            interactableElement.dependentInteractivity.SetInteractable(false);
        }

        [OnEventFire]
        public void HandleAcceptableState(NodeAddedEvent e, [Combine] InteractivityPrerequisiteStates.AcceptableState prerequisiteNode,
            [JoinByScreen] [Combine] [Context] DependentInteractivityNode interactableElementNode) {
            DependentInteractivityComponent dependentInteractivity = interactableElementNode.dependentInteractivity;

            for (int i = 0; i < dependentInteractivity.prerequisitesObjects.Count; i++) {
                GameObject gameObject = dependentInteractivity.prerequisitesObjects[i].gameObject;

                if (gameObject.activeInHierarchy) {
                    EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();

                    if (component.Entity != null && component.Entity.HasComponent<NotAcceptableStateComponent>()) {
                        dependentInteractivity.SetInteractable(false);
                        return;
                    }
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
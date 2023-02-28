using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class AttachToScreenSystem : ECSSystem {
        [OnEventFire]
        public void AttachToScreen(NodeAddedEvent e, SingleNode<AttachToScreenComponent> screenElement, [JoinAll] ScreenNode screen) {
            AttachToScreenComponent component = screenElement.component;
            screen.screenGroup.Attach(screenElement.Entity);
        }

        [OnEventFire]
        public void AddAttachComponent(NodeAddedEvent e, ScreenNode screenNode) {
            AttachChildren(screenNode.screen.gameObject, screenNode);
        }

        [OnEventFire]
        public void AddAttachComponent(NodeAddedEvent e, DialogNode dialogNode) {
            AttachChildren(dialogNode.dialogs60.gameObject, dialogNode);
        }

        void AttachChildren(GameObject gameObject, ScreenGroupNode node) {
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            EntityBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<EntityBehaviour>(true);

            foreach (EntityBehaviour entityBehaviour in componentsInChildren) {
                if (entityBehaviour.Equals(component)) {
                    continue;
                }

                AttachToScreenComponent component2 = entityBehaviour.gameObject.GetComponent<AttachToScreenComponent>();

                if (component2 == null) {
                    AttachToScreenComponent attachToScreenComponent = entityBehaviour.gameObject.AddComponent<AttachToScreenComponent>();
                    attachToScreenComponent.JoinEntityBehaviour = component;
                    Entity entity = entityBehaviour.Entity;

                    if (entity != null && entityBehaviour.handleAutomaticaly) {
                        ((EntityInternal)entity).AddComponent(attachToScreenComponent);
                    }
                } else if (entityBehaviour.Entity != null) {
                    if (entityBehaviour.Entity.HasComponent<ScreenGroupComponent>()) {
                        entityBehaviour.Entity.RemoveComponent<ScreenGroupComponent>();
                    }

                    node.screenGroup.Attach(entityBehaviour.Entity);
                }
            }
        }

        public class ScreenGroupNode : Node {
            public ScreenGroupComponent screenGroup;
        }

        public class ScreenNode : ScreenGroupNode {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class DialogNode : ScreenGroupNode {
            public Dialogs60Component dialogs60;
        }
    }
}
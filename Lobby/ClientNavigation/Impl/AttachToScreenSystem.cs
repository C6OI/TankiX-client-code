using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientNavigation.Impl {
    public class AttachToScreenSystem : ECSSystem {
        [OnEventFire]
        public void AttachToScreen(NodeAddedEvent e, SingleNode<AttachToScreenComponent> screenElement,
            [JoinAll] ScreenNode screen) {
            AttachToScreenComponent component = screenElement.component;
            screen.screenGroup.Attach(screenElement.Entity);
        }

        [OnEventFire]
        public void AddAttachComponent(NodeAddedEvent e, ScreenNode screenNode) {
            ScreenComponent screen = screenNode.screen;
            EntityBehaviour component = screen.GetComponent<EntityBehaviour>();
            EntityBehaviour[] componentsInChildren = screen.gameObject.GetComponentsInChildren<EntityBehaviour>(true);

            foreach (EntityBehaviour entityBehaviour in componentsInChildren) {
                if (entityBehaviour.Equals(component)) {
                    continue;
                }

                AttachToScreenComponent component2 = entityBehaviour.gameObject.GetComponent<AttachToScreenComponent>();

                if (component2 == null) {
                    AttachToScreenComponent attachToScreenComponent =
                        entityBehaviour.gameObject.AddComponent<AttachToScreenComponent>();

                    attachToScreenComponent.JoinEntityBehaviour = component;
                    Entity entity = entityBehaviour.Entity;

                    if (entity != null && entityBehaviour.handleAutomaticaly) {
                        ((EntityInternal)entity).AddComponent(attachToScreenComponent);
                    }
                } else if (entityBehaviour.Entity != null) {
                    screenNode.screenGroup.Attach(entityBehaviour.Entity);
                }
            }
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;

            public ScreenGroupComponent screenGroup;
        }
    }
}
using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityStateMachineImpl : EntityStateMachine {
        readonly IDictionary<Type, EntityState> entityStates;

        readonly HashSet<Type> removedComponents;

        public EntityState currentState;
        Entity entity;

        public EntityStateMachineImpl() {
            entityStates = new Dictionary<Type, EntityState>();
            removedComponents = new HashSet<Type>();
        }

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public void AddState<T>() where T : Node, new() {
            Type typeFromHandle = typeof(T);

            if (entityStates.ContainsKey(typeFromHandle)) {
                throw new EntityStateAlreadyRegisteredException(typeFromHandle);
            }

            EntityState entityState = new(typeFromHandle,
                NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeFromHandle).NodeDescription);

            if (entity != null) {
                entityState.Entity = entity;
            }

            entityStates[typeFromHandle] = entityState;
        }

        public T ChangeState<T>() where T : Node {
            Type typeFromHandle = typeof(T);
            return (T)ChangeState(typeFromHandle);
        }

        public Node ChangeState(Type stateType) {
            if (!entityStates.ContainsKey(stateType)) {
                throw new EntityStateNotRegisteredException(stateType);
            }

            EntityState entityState = entityStates[stateType];
            Node node = entityState.Node;

            if (currentState != entityState) {
                ClearComponents(node);
                EnterState(node);
                currentState = entityState;
            }

            return node;
        }

        public void AttachToEntity(Entity entity) {
            this.entity = entity;

            foreach (EntityState value in entityStates.Values) {
                value.Entity = entity;
            }
        }

        void EnterState(Node nextState) {
            EntityState entityState = entityStates[nextState.GetType()];
            ICollection<Type> components = entityState.Components;

            foreach (Type item in components) {
                if (!entity.HasComponent(item)) {
                    Component component = entity.CreateNewComponentInstance(item);
                    entityState.AssignValue(item, component);
                    entity.AddComponent(component);
                } else {
                    entityState.AssignValue(item, ((EntityInternal)entity).GetComponent(item));
                }
            }
        }

        void ClearComponents(Node nextState) {
            ICollection<Type> components = entityStates[nextState.GetType()].Components;

            foreach (EntityState value in entityStates.Values) {
                ICollection<Type> components2 = value.Components;

                foreach (Type item in components2) {
                    if (entity.HasComponent(item) && !components.Contains(item) && !removedComponents.Contains(item)) {
                        entity.RemoveComponent(item);
                        removedComponents.Add(item);
                    }
                }
            }

            removedComponents.Clear();
        }
    }
}
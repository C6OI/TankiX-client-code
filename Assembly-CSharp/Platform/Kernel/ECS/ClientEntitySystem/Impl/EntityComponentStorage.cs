using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityComponentStorage {
        readonly IDictionary<Type, Component> components = new Dictionary<Type, Component>();
        readonly EntityInternal entity;

        readonly ComponentBitIdRegistry componentBitIdRegistry;

        public EntityComponentStorage(EntityInternal entity, ComponentBitIdRegistry componentBitIdRegistry) {
            this.entity = entity;
            this.componentBitIdRegistry = componentBitIdRegistry;
            bitId = new BitSet();
        }

        public BitSet bitId { get; }

        public ICollection<Type> ComponentClasses => components.Keys;

        public ICollection<Component> Components => components.Values;

        public void AddComponentsImmediately(IList<Component> addedComponents) {
            addedComponents.ForEach(delegate(Component component) {
                AddComponentImmediately(component.GetType(), component);
            });
        }

        public void AddComponentImmediately(Type comType, Component component) {
            try {
                components.Add(comType, component);
                bitId.Set(componentBitIdRegistry.GetComponentBitId(comType));
            } catch (ArgumentException) {
                throw new ComponentAlreadyExistsInEntityException(entity, comType);
            }
        }

        public bool HasComponent(Type componentClass) => components.ContainsKey(componentClass);

        public Component GetComponent(Type componentClass) {
            try {
                return components[componentClass];
            } catch (KeyNotFoundException) {
                throw new ComponentNotFoundException(entity, componentClass);
            }
        }

        public Component GetComponentUnsafe(Type componentType) {
            Component value;
            return !components.TryGetValue(componentType, out value) ? null : value;
        }

        public void ChangeComponent(Component component) {
            Type type = component.GetType();
            AssertComponentFound(type);
            components[type] = component;
        }

        public Component RemoveComponentImmediately(Type componentClass) {
            try {
                Component result = components[componentClass];
                components.Remove(componentClass);
                bitId.Clear(componentBitIdRegistry.GetComponentBitId(componentClass));
                return result;
            } catch (KeyNotFoundException) {
                throw new ComponentNotFoundException(entity, componentClass);
            }
        }

        void AssertComponentFound(Type componentClass) {
            if (!components.ContainsKey(componentClass)) {
                throw new ComponentNotFoundException(entity, componentClass);
            }
        }

        public void OnEntityDelete() {
            components.Clear();
            bitId.Clear();
        }
    }
}
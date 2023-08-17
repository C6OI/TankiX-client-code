using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityStub : Entity, EntityInternal {
        readonly Node node;

        public EntityStub() {
            node = new Node();
            node.Entity = this;
        }

        public long Id => -1L;

        public string Name {
            get => "Stub";
            set => throw new NotSupportedException();
        }

        public void AddComponent(Type componentType) => throw new NotSupportedException("ComponentType: " + componentType);

        public Component CreateNewComponentInstance(Type componentType) =>
            throw new NotSupportedException("ComponentType: " + componentType);

        public void AddComponent(Component component) =>
            throw new NotSupportedException("ComponentType: " + component.GetType());

        public void AddComponent<T>() where T : Component, new() =>
            throw new NotSupportedException("ComponentType: " + typeof(T));

        public Component GetComponent(Type componentType) =>
            throw new NotSupportedException("ComponentType: " + componentType);

        public T GetComponent<T>() where T : Component => throw new NotSupportedException("ComponentType: " + typeof(T));

        public void RemoveComponent(Type componentType) =>
            throw new NotSupportedException("ComponentType: " + componentType);

        public void RemoveComponent<T>() where T : Component =>
            throw new NotSupportedException("ComponentType: " + typeof(T));

        public bool HasComponent<T>() where T : Component => false;

        public bool HasComponent(Type type) => false;

        public T CreateGroup<T>() where T : GroupComponent => throw new NotSupportedException("ComponentType: " + typeof(T));

        public T ToNode<T>() where T : Node, new() {
            if (typeof(T) == typeof(Node)) {
                return (T)node;
            }

            throw new NotSupportedException();
        }

        public T AddComponentAndGetInstance<T>() => throw new NotSupportedException("ComponentType: " + typeof(T));

        public NodeDescriptionStorage NodeDescriptionStorage => throw new NotSupportedException();

        public BitSet ComponentsBitId => throw new NotSupportedException();

        public ICollection<Type> ComponentClasses => throw new NotSupportedException();

        public ICollection<Component> Components => throw new NotSupportedException();

        public Optional<TemplateAccessor> TemplateAccessor {
            get => Optional<TemplateAccessor>.empty();
            set => throw new NotSupportedException();
        }

        public bool Alive => true;

        public void NotifyComponentChange(Type componentType) => throw new NotSupportedException();

        public void AddEntityListener(EntityListener entityListener) => throw new NotSupportedException();

        public void RemoveEntityListener(EntityListener entityListener) => throw new NotSupportedException();

        public bool Contains(NodeDescription node) => node.IsEmpty;

        public string ToStringWithComponentsClasses() => throw new NotSupportedException();

        public void AddComponentWithoutEvent(Component component) => throw new NotSupportedException();

        public void RemoveComponentWithoutEvent(Type componentType) => throw new NotSupportedException();

        public void Init() => throw new NotSupportedException();

        public void ChangeComponent(Component component) =>
            throw new NotSupportedException("ComponentType: " + component.GetType());

        public void OnDelete() => throw new NotSupportedException();

        public bool CanCast(NodeDescription desc) {
            if (desc.IsEmpty) {
                return true;
            }

            return false;
        }

        public Node GetNode(NodeClassInstanceDescription instanceDescription) {
            if (instanceDescription.NodeDescription.IsEmpty) {
                return node;
            }

            throw new NotSupportedException();
        }
    }
}
using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EntityInternal : Entity {
        ICollection<Type> ComponentClasses { get; }

        ICollection<Component> Components { get; }

        Optional<TemplateAccessor> TemplateAccessor { get; set; }

        bool Alive { get; }

        NodeDescriptionStorage NodeDescriptionStorage { get; }

        BitSet ComponentsBitId { get; }

        void Init();

        void ChangeComponent(Component component);

        void OnDelete();

        bool CanCast(NodeDescription desc);

        Node GetNode(NodeClassInstanceDescription instanceDescription);

        new void AddComponent(Component component);

        void NotifyComponentChange(Type componentType);

        void AddEntityListener(EntityListener entityListener);

        void RemoveEntityListener(EntityListener entityListener);

        bool Contains(NodeDescription node);

        string ToStringWithComponentsClasses();

        void AddComponentWithoutEvent(Component component);

        void RemoveComponentWithoutEvent(Type componentType);
    }
}
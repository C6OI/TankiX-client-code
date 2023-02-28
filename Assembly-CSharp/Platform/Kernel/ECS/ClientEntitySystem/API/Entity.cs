using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface Entity {
        long Id { get; }

        string Name { get; set; }

        string ConfigPath { get; }

        bool Alive { get; }

        void AddComponent(Type componentType);

        Component CreateNewComponentInstance(Type componentType);

        void AddComponent(Component component);

        void AddComponent<T>() where T : Component, new();

        void AddComponentIfAbsent<T>() where T : Component, new();

        Component GetComponent(Type componentType);

        T GetComponent<T>() where T : Component;

        void RemoveComponent(Type componentType);

        void RemoveComponent<T>() where T : Component;

        void RemoveComponentIfPresent<T>() where T : Component;

        bool HasComponent<T>() where T : Component;

        bool HasComponent(Type type);

        T CreateGroup<T>() where T : GroupComponent;

        T ToNode<T>() where T : Node, new();

        T AddComponentAndGetInstance<T>();

        bool IsSameGroup<T>(Entity otherEntity) where T : GroupComponent;

        void ScheduleEvent<T>() where T : Event, new();

        void ScheduleEvent(Event eventInstance);

        T SendEvent<T>(T eventInstance) where T : Event;
    }
}
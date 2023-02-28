using System;
using System.Collections.Generic;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface Engine {
        Entity CreateEntity(string name);

        Entity CreateEntity<T>() where T : Template;

        Entity CreateEntity<T>(YamlNode yamlNode) where T : Template;

        Entity CreateEntity<T>(string configPath) where T : Template;

        Entity CreateEntity(Type templateType, string configPath);

        Entity CreateEntity<T>(string configPath, long id) where T : Template;

        Entity CreateEntity(long templateId, string configPath, long id);

        Entity CreateEntity(long templateId, string configPath);

        Entity CloneEntity(string name, Entity entity);

        ICollection<Entity> CreateEntities<T>(string configPathWithWildcard) where T : Template;

        void DeleteEntity(Entity entity);

        EventBuilder NewEvent(Event eventInstance);

        EventBuilder NewEvent<T>() where T : Event, new();

        void ScheduleEvent<T>() where T : Event, new();

        void ScheduleEvent<T>(Entity entity) where T : Event, new();

        void ScheduleEvent<T>(Node node) where T : Event, new();

        void ScheduleEvent<T>(GroupComponent group) where T : Event, new();

        void ScheduleEvent(Event eventInstance);

        void ScheduleEvent(Event eventInstance, Entity entity);

        void ScheduleEvent(Event eventInstance, Node node);

        void ScheduleEvent(Event eventInstance, GroupComponent group);

        IList<T> Select<T>(Entity entity, Type groupComponentType) where T : Node;

        ICollection<T> SelectAll<T>() where T : Node;

        ICollection<Entity> SelectAllEntities<T>() where T : Node;
    }
}
using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Library.ClientUnityIntegration.API {
    public abstract class ECSBehaviour : MonoBehaviour, Engine {
        [Inject] public static EngineService EngineService { get; set; }

        public Entity CreateEntity(string name) => EngineService.Engine.CreateEntity(name);

        public Entity CreateEntity<T>() where T : Template => EngineService.Engine.CreateEntity<T>();

        public Entity CreateEntity<T>(YamlNode yamlNode) where T : Template => EngineService.Engine.CreateEntity<T>(yamlNode);

        public Entity CreateEntity<T>(string configPath) where T : Template => EngineService.Engine.CreateEntity<T>(configPath);

        public Entity CreateEntity(Type templateType, string configPath) => EngineService.Engine.CreateEntity(templateType, configPath);

        public Entity CreateEntity<T>(string configPath, long id) where T : Template => EngineService.Engine.CreateEntity<T>(configPath, id);

        public Entity CreateEntity(long templateId, string configPath, long id) => EngineService.Engine.CreateEntity(templateId, configPath, id);

        public Entity CreateEntity(long templateId, string configPath) => EngineService.Engine.CreateEntity(templateId, configPath);

        public Entity CloneEntity(string name, Entity entity) => EngineService.Engine.CloneEntity(name, entity);

        public ICollection<Entity> CreateEntities<T>(string configPathWithWildcard) where T : Template => EngineService.Engine.CreateEntities<T>(configPathWithWildcard);

        public void DeleteEntity(Entity entity) {
            EngineService.Engine.DeleteEntity(entity);
        }

        public EventBuilder NewEvent(Event eventInstance) => EngineService.Engine.NewEvent(eventInstance);

        public EventBuilder NewEvent<T>() where T : Event, new() => EngineService.Engine.NewEvent<T>();

        public void ScheduleEvent<T>() where T : Event, new() {
            EngineService.Engine.ScheduleEvent<T>();
        }

        public void ScheduleEvent<T>(Entity entity) where T : Event, new() {
            EngineService.Engine.ScheduleEvent<T>(entity);
        }

        public void ScheduleEvent<T>(Node node) where T : Event, new() {
            EngineService.Engine.ScheduleEvent<T>(node);
        }

        public void ScheduleEvent<T>(GroupComponent group) where T : Event, new() {
            EngineService.Engine.ScheduleEvent<T>(group);
        }

        public void ScheduleEvent(Event eventInstance) {
            EngineService.Engine.ScheduleEvent(eventInstance);
        }

        public void ScheduleEvent(Event eventInstance, Entity entity) {
            EngineService.Engine.ScheduleEvent(eventInstance, entity);
        }

        public void ScheduleEvent(Event eventInstance, Node node) {
            EngineService.Engine.ScheduleEvent(eventInstance, node);
        }

        public void ScheduleEvent(Event eventInstance, GroupComponent group) {
            EngineService.Engine.ScheduleEvent(eventInstance, group);
        }

        public IList<T> Select<T>(Entity entity, Type groupComponentType) where T : Node => EngineService.Engine.Select<T>(entity, groupComponentType);

        public ICollection<T> SelectAll<T>() where T : Node => EngineService.Engine.SelectAll<T>();

        public ICollection<Entity> SelectAllEntities<T>() where T : Node => EngineService.Engine.SelectAllEntities<T>();
    }
}
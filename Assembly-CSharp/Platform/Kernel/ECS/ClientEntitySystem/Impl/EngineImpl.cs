using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EngineImpl : Engine {
        readonly Entity _fakeEntity = new EntityStub();
        DelayedEventManager delayedEventManager;

        EntityCloner entityCloner;

        TemplateRegistry templateRegistry;

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject] public static FlowInstancesCache Cache { get; set; }

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public Entity CreateEntity(string name) => EngineService.CreateEntityBuilder().SetName(name).Build();

        public Entity CreateEntity<T>() where T : Template => EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).Build();

        public Entity CreateEntity<T>(YamlNode yamlNode) where T : Template => EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).SetTemplateYamlNode(yamlNode)
            .Build();

        public Entity CreateEntity<T>(string configPath) where T : Template => EngineService.CreateEntityBuilder().SetConfig(configPath).SetTemplate(typeof(T))
            .Build();

        public Entity CreateEntity(Type templateType, string configPath) => EngineService.CreateEntityBuilder().SetConfig(configPath).SetTemplate(templateType)
            .Build();

        public Entity CreateEntity<T>(string configPath, long id) where T : Template => EngineService.CreateEntityBuilder().SetId(id).SetConfig(configPath)
            .SetTemplate(typeof(T))
            .Build();

        public Entity CreateEntity(long templateId, string configPath, long id) => EngineService.CreateEntityBuilder().SetId(id).SetTemplate(templateRegistry.GetTemplateInfo(templateId))
            .SetConfig(configPath)
            .Build();

        public Entity CreateEntity(long templateId, string configPath) => EngineService.CreateEntityBuilder().SetTemplate(templateRegistry.GetTemplateInfo(templateId))
            .SetConfig(configPath)
            .Build();

        public ICollection<Entity> CreateEntities<T>(string configPathWithWildcard) where T : Template {
            List<Entity> list = new();

            foreach (string item2 in ConfigurationService.GetPathsByWildcard(configPathWithWildcard)) {
                Entity item = EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).SetConfig(item2)
                    .Build();

                list.Add(item);
            }

            return list;
        }

        public Entity CloneEntity(string name, Entity entity) => entityCloner.Clone(name, (EntityInternal)entity, EngineService.CreateEntityBuilder());

        public void DeleteEntity(Entity entity) {
            entity.AddComponent<DeletedEntityComponent>();
            ScheduleEvent<DeleteEntityEvent>(entity);
            Flow.Current.EntityRegistry.Remove(entity.Id);
            ((EntityInternal)entity).OnDelete();
        }

        public EventBuilder NewEvent(Event eventInstance) => Cache.eventBuilder.GetInstance().Init(delayedEventManager, Flow.Current, eventInstance);

        public EventBuilder NewEvent<T>() where T : Event, new() => NewEvent(CreateOrReuseEventInstance<T>());

        public void ScheduleEvent<T>() where T : Event, new() {
            ScheduleEvent<T>(_fakeEntity);
        }

        public void ScheduleEvent<T>(Node node) where T : Event, new() {
            ScheduleEvent<T>(node.Entity);
        }

        public void ScheduleEvent<T>(Entity entity) where T : Event, new() {
            NewEvent(CreateOrReuseEventInstance<T>()).Attach(entity).Schedule();
        }

        public void ScheduleEvent<T>(GroupComponent group) where T : Event, new() {
            ICollection<Entity> groupMembers = group.GetGroupMembers();
            NewEvent(CreateOrReuseEventInstance<T>()).AttachAll(groupMembers).Schedule();
        }

        public void ScheduleEvent(Event eventInstance) {
            ScheduleEvent(eventInstance, _fakeEntity);
        }

        public void ScheduleEvent(Event eventInstance, Node node) {
            ScheduleEvent(eventInstance, node.Entity);
        }

        public void ScheduleEvent(Event eventInstance, Entity entity) {
            NewEvent(eventInstance).Attach(entity).Schedule();
        }

        public void ScheduleEvent(Event eventInstance, GroupComponent group) {
            ICollection<Entity> groupMembers = group.GetGroupMembers();
            NewEvent(eventInstance).AttachAll(groupMembers).Schedule();
        }

        public IList<N> Select<N>(Entity entity, Type groupComponentType) where N : Node {
            if (!typeof(GroupComponent).IsAssignableFrom(groupComponentType)) {
                throw new NotGroupComponentException(groupComponentType);
            }

            return DoSelect<N>(entity, groupComponentType);
        }

        public ICollection<N> SelectAll<N>() where N : Node {
            NodeClassInstanceDescription nodeDesc = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N));

            return (from e in SelectAllEntities<N>()
                    select (N)((EntityInternal)e).GetNode(nodeDesc)).ToList();
        }

        public ICollection<Entity> SelectAllEntities<N>() where N : Node {
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N));
            return Flow.Current.NodeCollector.GetEntities(orCreateNodeClassDescription.NodeDescription);
        }

        public virtual void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager) {
            this.templateRegistry = templateRegistry;
            this.delayedEventManager = delayedEventManager;
            entityCloner = new EntityCloner();
        }

        T CreateOrReuseEventInstance<T>() where T : Event, new() {
            Event instance;

            if (EngineService.EventInstancesStorageForReuse.TryGetInstance(typeof(T), out instance)) {
                return (T)instance;
            }

            return new T();
        }

        public void OnDeleteEntity(Entity entity) {
            Flow.Current.EntityRegistry.Remove(entity.Id);
            ((EntityInternal)entity).OnDelete();
        }

        public IList<N> Select<N, G>(Entity entity) where N : Node where G : GroupComponent => DoSelect<N>(entity, typeof(G));

        IList<N> DoSelect<N>(Entity entity, Type groupComponentType) where N : Node {
            GroupComponent groupComponent;

            if ((groupComponent = (GroupComponent)((EntityUnsafe)entity).GetComponentUnsafe(groupComponentType)) == null) {
                return Collections.EmptyList<N>();
            }

            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N));
            NodeDescriptionRegistry.AssertRegister(orCreateNodeClassDescription.NodeDescription);
            ICollection<Entity> groupMembers;

            if ((groupMembers = groupComponent.GetGroupMembers(orCreateNodeClassDescription.NodeDescription)).Count == 0) {
                return Collections.EmptyList<N>();
            }

            if (groupMembers.Count == 1) {
                return Collections.SingletonList((N)GetNode(Collections.GetOnlyElement(groupMembers), orCreateNodeClassDescription));
            }

            return (IList<N>)ConvertNodeCollection(orCreateNodeClassDescription, groupMembers);
        }

        IList ConvertNodeCollection(NodeClassInstanceDescription nodeClassInstanceDescription, ICollection<Entity> entities) {
            int count = entities.Count;
            IList genericListInstance = Cache.GetGenericListInstance(nodeClassInstanceDescription.NodeClass, count);
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Node node = GetNode(enumerator.Current, nodeClassInstanceDescription);
                genericListInstance.Add(node);
            }

            return genericListInstance;
        }

        Node GetNode(Entity entity, NodeClassInstanceDescription nodeClassInstanceDescription) => ((EntityInternal)entity).GetNode(nodeClassInstanceDescription);
    }
}
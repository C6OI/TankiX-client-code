using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityImpl : EntityInternal, EntityUnsafe, IComparable<EntityImpl>, Entity {
        public static readonly List<Entity> EMPTY_LIST = new();

        static readonly Type[] EmptyTypes = new Type[0];

        protected readonly EngineServiceInternal engineService;

        readonly int hashCode;

        protected readonly NodeDescriptionStorage nodeDescriptionStorage;

        readonly NodeProvider nodeProvider;

        protected readonly EntityComponentStorage storage;

        ICollection<EntityListener> entityListeners;

        protected NodeChangedEventMaker nodeAddedEventMaker;

        readonly NodeCache nodeCache;

        protected NodeChangedEventMaker nodeRemoveEventMaker;

        public EntityImpl(EngineServiceInternal engineService, long id, string name)
            : this(engineService, id, name, Optional<TemplateAccessor>.empty()) { }

        public EntityImpl(EngineServiceInternal engineService, long id, string name, Optional<TemplateAccessor> templateAccessor) {
            this.engineService = engineService;
            Id = id;
            Name = name;
            nodeCache = engineService.NodeCache;
            TemplateAccessor = templateAccessor;
            storage = new EntityComponentStorage(this, engineService.ComponentBitIdRegistry);
            nodeProvider = new NodeProvider(this);
            nodeDescriptionStorage = new NodeDescriptionStorage();
            hashCode = calcHashCode();
            nodeAddedEventMaker = new NodeChangedEventMaker(NodeAddedEvent.Instance, typeof(NodeAddedFireHandler), typeof(NodeAddedCompleteHandler), engineService.HandlerCollector);
            nodeRemoveEventMaker = new NodeChangedEventMaker(NodeRemoveEvent.Instance, typeof(NodeRemovedFireHandler), typeof(NodeRemovedCompleteHandler), engineService.HandlerCollector);
            Init();
            UpdateNodes(NodeDescriptionRegistry.GetNodeDescriptionsWithNotComponentsOnly());
        }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject] public static GroupRegistry GroupRegistry { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static FlowInstancesCache FlowInstancesCache { get; set; }

        public long Id { get; }

        public string Name { get; set; }

        public string ConfigPath => TemplateAccessor.Get().ConfigPath;

        public Optional<TemplateAccessor> TemplateAccessor { get; set; }

        public bool Alive { get; private set; }

        public ICollection<Component> Components => storage.Components;

        public ICollection<Type> ComponentClasses => storage.ComponentClasses;

        public NodeDescriptionStorage NodeDescriptionStorage => nodeDescriptionStorage;

        public BitSet ComponentsBitId => storage.bitId;

        public void Init() {
            Alive = true;
            entityListeners = new List<EntityListener> { engineService.HandlerContextDataStorage, engineService.HandlerStateListener, engineService.BroadcastEventHandlerCollector };
        }

        public void AddComponent<T>() where T : Component, new() {
            AddComponent(typeof(T));
        }

        public void AddComponentIfAbsent<T>() where T : Component, new() {
            if (!HasComponent<T>()) {
                AddComponent(typeof(T));
            }
        }

        public void AddComponent(Type componentType) {
            Component component = CreateNewComponentInstance(componentType);
            AddComponent(component);
        }

        public void AddComponent(Component component) {
            if (component is GroupComponent) {
                component = GroupRegistry.FindOrRegisterGroup((GroupComponent)component);
            }

            AddComponent(component, true);
        }

        public void AddComponentSilent(Component component) {
            AddComponent(component, false);
        }

        public void ChangeComponent(Component component) {
            bool flag = HasComponent(component.GetType()) && GetComponent(component.GetType()).Equals(component);
            storage.ChangeComponent(component);

            if (!flag) {
                nodeProvider.OnComponentChanged(component);
            }

            NotifyChangedInEntity(component);
        }

        public Component GetComponent(Type componentType) => storage.GetComponent(componentType);

        public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));

        public void OnDelete() {
            Alive = false;
            ClearNodes();
            SendEntityDeletedForAllListeners();
            storage.OnEntityDelete();
        }

        public bool CanCast(NodeDescription desc) => nodeProvider.CanCast(desc);

        public Node GetNode(NodeClassInstanceDescription instanceDescription) => nodeProvider.GetNode(instanceDescription);

        public void NotifyComponentChange(Type componentType) {
            Component component = GetComponent(componentType);
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator(engineService.ComponentListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnComponentChanged(this, component);
            }
        }

        public void RemoveComponent<T>() where T : Component {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponentIfPresent<T>() where T : Component {
            if (HasComponent<T>()) {
                RemoveComponent(typeof(T));
            }
        }

        public void RemoveComponent(Type componentType) {
            UpdateHandlers(componentType);
            NotifyComponentRemove(componentType);
            RemoveComponentSilent(componentType);
        }

        public void RemoveComponentSilent(Type componentType) {
            if (HasComponent(componentType) || !HasComponent<DeletedEntityComponent>() && !IsSkipExceptionOnAddRemove(componentType)) {
                SendNodeRemoved(componentType);
                Component component = storage.RemoveComponentImmediately(componentType);
                NotifyDetachFromEntity(component);
                UpdateNodesOnComponentRemoved(componentType);
            }
        }

        public bool HasComponent<T>() where T : Component => HasComponent(typeof(T));

        public bool HasComponent(Type type) => storage.HasComponent(type);

        public T CreateGroup<T>() where T : GroupComponent {
            T val = GroupRegistry.FindOrCreateGroup<T>(Id);
            AddComponent(val);
            return val;
        }

        public T ToNode<T>() where T : Node, new() => new() {
            Entity = this
        };

        public T AddComponentAndGetInstance<T>() {
            Component component = CreateNewComponentInstance(typeof(T));
            AddComponent(component);
            return (T)component;
        }

        public Component CreateNewComponentInstance(Type componentType) {
            Collections.Enumerator<ComponentConstructor> enumerator = Collections.GetEnumerator(engineService.ComponentConstructors);

            while (enumerator.MoveNext()) {
                ComponentConstructor current = enumerator.Current;

                if (current.IsAcceptable(componentType, this)) {
                    return current.GetComponentInstance(componentType, this);
                }
            }

            ConstructorInfo constructor = componentType.GetConstructor(EmptyTypes);
            return (Component)constructor.Invoke(Collections.EmptyArray);
        }

        public bool Contains(NodeDescription node) {
            BitSet componentsBitId = ComponentsBitId;
            return componentsBitId.Mask(node.NodeComponentBitId) && componentsBitId.MaskNot(node.NotNodeComponentBitId);
        }

        public string ToStringWithComponentsClasses() {
            string[] value = (from c in ComponentClasses
                              select c.Name
                              into c
                              orderby c
                              select c).ToArray();

            return string.Format("Entity[id={0}, name={1}, components={2}]", Id, Name, string.Join(",", value));
        }

        public bool IsSameGroup<T>(Entity otherEntity) where T : GroupComponent {
            if (HasComponent<T>() && otherEntity.HasComponent<T>()) {
                T component = GetComponent<T>();
                long key = component.Key;
                T component2 = otherEntity.GetComponent<T>();
                return key.Equals(component2.Key);
            }

            return false;
        }

        public void AddEntityListener(EntityListener entityListener) {
            entityListeners.Add(entityListener);
        }

        public virtual void RemoveEntityListener(EntityListener entityListener) {
            entityListeners.Remove(entityListener);
        }

        public void ScheduleEvent<T>() where T : Event, new() {
            EngineService.Engine.ScheduleEvent<T>(this);
        }

        public void ScheduleEvent(Event eventInstance) {
            EngineService.Engine.ScheduleEvent(eventInstance, this);
        }

        public T SendEvent<T>(T eventInstance) where T : Event {
            EngineService.Engine.ScheduleEvent(eventInstance, this);
            return eventInstance;
        }

        public Component GetComponentUnsafe(Type componentType) => storage.GetComponentUnsafe(componentType);

        public int CompareTo(EntityImpl other) => (int)(Id - other.Id);

        void AddComponent(Component component, bool sendEvent) {
            Type type = component.GetType();

            if (!storage.HasComponent(type) || !IsSkipExceptionOnAddRemove(type)) {
                UpdateHandlers(component.GetType());
                NotifyAttachToEntity(component);
                storage.AddComponentImmediately(component.GetType(), component);
                MakeNodes(component.GetType(), component);

                if (sendEvent) {
                    NotifyAddComponent(component);
                }

                PrepareAndSendNodeAddedEvent(component);
            }
        }

        void NotifyAddComponent(Component component) {
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator(engineService.ComponentListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnComponentAdded(this, component);
            }
        }

        void UpdateHandlers(Type componentType) {
            if (componentType.IsSubclassOf(typeof(GroupComponent))) {
                engineService.HandlerCollector.GetHandlersByGroupComponent(componentType).ForEach(delegate(Handler h) {
                    h.ChangeVersion();
                });
            }
        }

        void PrepareAndSendNodeAddedEvent(Component component) {
            nodeAddedEventMaker.MakeIfNeed(this, component.GetType());
        }

        void NotifyAttachToEntity(Component component) {
            AttachToEntityListener attachToEntityListener = component as AttachToEntityListener;

            if (attachToEntityListener != null) {
                attachToEntityListener.AttachedToEntity(this);
            }
        }

        void NotifyChangedInEntity(Component component) {
            ComponentServerChangeListener componentServerChangeListener = component as ComponentServerChangeListener;

            if (componentServerChangeListener != null) {
                componentServerChangeListener.ChangedOnServer(this);
            }
        }

        void SendEntityDeletedForAllListeners() {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator(entityListeners);

            while (enumerator.MoveNext()) {
                EntityListener current = enumerator.Current;
                current.OnEntityDeleted(this);
            }

            entityListeners.Clear();
        }

        void NotifyComponentRemove(Type componentType) {
            Component component = storage.GetComponent(componentType);
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator(engineService.ComponentListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnComponentRemoved(this, component);
            }
        }

        bool IsSkipExceptionOnAddRemove(Type componentType) => componentType.IsDefined(typeof(SkipExceptionOnAddRemoveAttribute), true);

        void SendNodeRemoved(Type componentType) {
            nodeRemoveEventMaker.MakeIfNeed(this, componentType);
        }

        void NotifyDetachFromEntity(Component component) {
            DetachFromEntityListener detachFromEntityListener = component as DetachFromEntityListener;

            if (detachFromEntityListener != null) {
                detachFromEntityListener.DetachedFromEntity(this);
            }
        }

        protected void MakeNodes(Type componentType, Component component) {
            nodeProvider.OnComponentAdded(component, componentType);
            NodesToChange nodesToChange = nodeCache.GetNodesToChange(this, componentType);

            foreach (NodeDescription item in nodesToChange.NodesToAdd) {
                AddNode(item);
            }

            foreach (NodeDescription item2 in nodesToChange.NodesToRemove) {
                RemoveNode(item2);
            }
        }

        void UpdateNodes(ICollection<NodeDescription> nodes) {
            BitSet componentsBitId = ComponentsBitId;
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(nodes);

            while (enumerator.MoveNext()) {
                NodeDescription current = enumerator.Current;

                if (componentsBitId.Mask(current.NodeComponentBitId)) {
                    if (componentsBitId.MaskNot(current.NotNodeComponentBitId)) {
                        AddNode(current);
                    } else if (nodeDescriptionStorage.Contains(current)) {
                        RemoveNode(current);
                    }
                }
            }
        }

        protected internal void AddNode(NodeDescription node) {
            Flow.Current.NodeCollector.Attach(this, node);
            nodeDescriptionStorage.AddNode(node);
            SendNodeAddedForCollectors(node);
        }

        void UpdateNodesOnComponentRemoved(Type componentClass) {
            List<NodeDescription> list = new(nodeDescriptionStorage.GetNodeDescriptions(componentClass));
            list.ForEach(RemoveNode);
            UpdateNodes(NodeDescriptionRegistry.GetNodeDescriptionsByNotComponent(componentClass));
        }

        void ClearNodes() {
            List<NodeDescription> list = new(nodeDescriptionStorage.GetNodeDescriptions());
            list.ForEach(RemoveNode);
            nodeProvider.CleanNodes();
        }

        internal void RemoveNode(NodeDescription node) {
            SendNodeRemovedForCollectors(node);
            Flow.Current.NodeCollector.Detach(this, node);
            nodeDescriptionStorage.RemoveNode(node);
        }

        void SendNodeAddedForCollectors(NodeDescription nodeDescription) {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator(entityListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnNodeAdded(this, nodeDescription);
            }
        }

        void SendNodeRemovedForCollectors(NodeDescription nodeDescription) {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator(entityListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnNodeRemoved(this, nodeDescription);
            }
        }

        protected bool Equals(EntityImpl other) => Id == other.Id;

        public override bool Equals(object obj) {
            if (obj is Node) {
                obj = ((Node)obj).Entity;
            }

            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != GetType()) {
                return false;
            }

            return Equals((EntityImpl)obj);
        }

        public override int GetHashCode() => hashCode;

        int calcHashCode() => Id.GetHashCode();

        public override string ToString() => string.Format("{0}({1})", Id, Name);
    }
}
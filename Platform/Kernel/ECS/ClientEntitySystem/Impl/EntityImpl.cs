using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityImpl : Entity, EntityInternal, EntityUnsafe, IComparable<EntityImpl> {
        public static readonly List<Entity> EMPTY_LIST = new();

        static readonly Type[] EmptyTypes = new Type[0];

        protected readonly EngineServiceInternal engineService;

        readonly int hashCode;

        readonly NodeCache nodeCache;

        protected readonly NodeDescriptionStorage nodeDescriptionStorage;

        readonly NodeProvider nodeProvider;

        protected readonly EntityComponentStorage storage;

        ICollection<EntityListener> entityListeners;

        protected NodeChangedEventMaker nodeAddedEventMaker;

        protected NodeChangedEventMaker nodeRemoveEventMaker;

        public EntityImpl(EngineServiceInternal engineService, long id, string name)
            : this(engineService, id, name, Optional<TemplateAccessor>.empty()) { }

        public EntityImpl(EngineServiceInternal engineService, long id, string name,
            Optional<TemplateAccessor> templateAccessor) {
            this.engineService = engineService;
            Id = id;
            Name = name;
            nodeCache = engineService.NodeCache;
            TemplateAccessor = templateAccessor;
            storage = new EntityComponentStorage(this, engineService.ComponentBitIdRegistry);
            nodeProvider = new NodeProvider(this);
            nodeDescriptionStorage = new NodeDescriptionStorage();
            hashCode = calcHashCode();

            nodeAddedEventMaker = new NodeChangedEventMaker(NodeAddedEvent.Instance,
                typeof(NodeAddedFireHandler),
                typeof(NodeAddedCompleteHandler),
                engineService.HandlerCollector);

            nodeRemoveEventMaker = new NodeChangedEventMaker(NodeRemoveEvent.Instance,
                typeof(NodeRemovedFireHandler),
                typeof(NodeRemovedCompleteHandler),
                engineService.HandlerCollector);

            Init();
            UpdateNodes(NodeDescriptionRegistry.GetNodeDescriptionsWithNotComponentsOnly());
        }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject] public static GroupRegistry GroupRegistry { get; set; }

        public long Id { get; }

        public string Name { get; set; }

        public void AddComponent<T>() where T : Component, new() => AddComponent(typeof(T));

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

        public Component GetComponent(Type componentType) => storage.GetComponent(componentType);

        public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));

        public void RemoveComponent<T>() where T : Component => RemoveComponent(typeof(T));

        public void RemoveComponent(Type componentType) {
            Flow.Current.SendEvent(new RemoveComponentEvent(componentType), this);
            RemoveComponentWithoutEvent(componentType);
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
            foreach (ComponentConstructor componentConstructor in engineService.ComponentConstructors) {
                if (componentConstructor.IsAcceptable(componentType, this)) {
                    return componentConstructor.GetComponentInstance(componentType, this);
                }
            }

            ConstructorInfo constructor = componentType.GetConstructor(EmptyTypes);
            return (Component)constructor.Invoke(Collections.EmptyArray);
        }

        public Optional<TemplateAccessor> TemplateAccessor { get; set; }

        public bool Alive { get; private set; }

        public ICollection<Component> Components => storage.Components;

        public ICollection<Type> ComponentClasses => storage.ComponentClasses;

        public NodeDescriptionStorage NodeDescriptionStorage => nodeDescriptionStorage;

        public BitSet ComponentsBitId => storage.bitId;

        public void Init() {
            Alive = true;

            entityListeners = new List<EntityListener> {
                engineService.HandlerContextDataStorage, engineService.HandlerStateListener,
                engineService.BroadcastEventHandlerCollector
            };
        }

        public void AddComponentWithoutEvent(Component component) => AddComponent(component, false);

        public void ChangeComponent(Component component) {
            storage.ChangeComponent(component);
            nodeProvider.OnComponentChanged(component);
            ComponentLifecycle componentLifecycle = component as ComponentLifecycle;

            if (componentLifecycle != null) {
                componentLifecycle.AttachToEntity(this);
            }
        }

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
            ChangeComponentEvent e = new(component);
            Flow.Current.SendEvent(e, this);
        }

        public void AddEntityListener(EntityListener entityListener) => entityListeners.Add(entityListener);

        public virtual void RemoveEntityListener(EntityListener entityListener) => entityListeners.Remove(entityListener);

        public void RemoveComponentWithoutEvent(Type componentType) {
            if (HasComponent(componentType) || !HasComponent<DeletedEntityComponent>()) {
                SendNodeRemoved(componentType);
                Component component = storage.RemoveComponentImmediately(componentType);
                NotifyDetachFromEntity(component);
                UpdateNodesOnComponentRemoved(componentType);
            }
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

        public Component GetComponentUnsafe(Type componentType) => storage.GetComponentUnsafe(componentType);

        public int CompareTo(EntityImpl other) => (int)(Id - other.Id);

        void AddComponent(Component component, bool sendEvent) {
            NotifyAttachToEntity(component);
            storage.AddComponentImmediately(component.GetType(), component);
            MakeNodes(component.GetType(), component);

            if (sendEvent) {
                Flow.Current.SendEvent(new ComponentAddedEvent(component), this);
            }

            SendNodeAdded(component);
        }

        void SendNodeAdded(Component component) => nodeAddedEventMaker.MakeIfNeed(this, component.GetType());

        void NotifyAttachToEntity(Component component) {
            ComponentLifecycle componentLifecycle = component as ComponentLifecycle;

            if (componentLifecycle != null) {
                componentLifecycle.AttachToEntity(this);
            }
        }

        void SendEntityDeletedForAllListeners() {
            entityListeners.ForEach(delegate(EntityListener l) {
                l.OnEntityDeleted(this);
            });

            entityListeners.Clear();
        }

        void SendNodeRemoved(Type componentType) => nodeRemoveEventMaker.MakeIfNeed(this, componentType);

        void NotifyDetachFromEntity(Component component) {
            ComponentLifecycle componentLifecycle = component as ComponentLifecycle;

            if (componentLifecycle != null) {
                componentLifecycle.DetachFromEntity(this);
            }
        }

        protected void MakeNodes(Type componentType, Component component) {
            nodeProvider.OnComponentAdded(component, componentType);
            NodesToChange nodesToChange = nodeCache.GetNodesToChange(this, componentType);
            nodesToChange.NodesToAdd.ForEach(AddNode);
            nodesToChange.NodesToRemove.ForEach(RemoveNode);
        }

        void UpdateNodes(ICollection<NodeDescription> nodes) {
            BitSet componentsBitId = ComponentsBitId;

            foreach (NodeDescription node in nodes) {
                if (componentsBitId.Mask(node.NodeComponentBitId)) {
                    if (componentsBitId.MaskNot(node.NotNodeComponentBitId)) {
                        AddNode(node);
                    } else if (nodeDescriptionStorage.Contains(node)) {
                        RemoveNode(node);
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

        void SendNodeAddedForCollectors(NodeDescription nodeDescription) => entityListeners.ForEach(
            delegate(EntityListener l) {
                l.OnNodeAdded(this, nodeDescription);
            });

        void SendNodeRemovedForCollectors(NodeDescription nodeDescription) => entityListeners.ForEach(
            delegate(EntityListener l) {
                l.OnNodeRemoved(this, nodeDescription);
            });

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

        public override string ToString() => string.Format("Entity[id={0}, name={1}]", Id, Name);
    }
}
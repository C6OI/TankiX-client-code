using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientDataStructures.Impl.Cache;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class FlowInstancesCache : AbstratFlowInstancesCache {
        public readonly Cache<ArrayList> arrayList;

        public readonly Cache<EntityNode> entityNode;

        public readonly Cache<EventBuilderImpl> eventBuilder;

        public readonly Cache<FlowHandlerInvokeDate> flowInvokeData;

        public readonly Cache<HandlerExecutor> handlerExecutor;
        public readonly Cache<List<Entity>> listEntity;

        public readonly Cache<List<Handler>> listHandlers;

        public readonly Cache<List<HandlerInvokeData>> listHandlersInvokeData;

        public readonly Cache<List<NodeDescription>> listNodeDescription;

        public readonly Cache<List<Type>> listType;

        public readonly Cache<HashSet<Entity>> setEntity;

        public readonly Cache<HashSet<NodeDescription>> setNodeDescription;

        public CacheMultisizeArray<object> array = new CacheMultisizeArrayImpl<object>();

        public CacheMultisizeArray<Entity> entityArray = new CacheMultisizeArrayImpl<Entity>();

        readonly Dictionary<Type, Type> genericListInstances = new();

        readonly Dictionary<Type, NodeInstanceCache> nodeInstancesCache = new();

        public FlowInstancesCache() {
            arrayList = Register(delegate(ArrayList list) {
                list.Clear();
            });

            listEntity = Register(delegate(List<Entity> list) {
                list.Clear();
            });

            setEntity = Register(delegate(HashSet<Entity> set) {
                set.Clear();
            });

            listType = Register(delegate(List<Type> list) {
                list.Clear();
            });

            setNodeDescription = Register(delegate(HashSet<NodeDescription> set) {
                set.Clear();
            });

            listNodeDescription = Register(delegate(List<NodeDescription> list) {
                list.Clear();
            });

            listHandlersInvokeData = Register(delegate(List<HandlerInvokeData> list) {
                list.Clear();
            });

            flowInvokeData = Register<FlowHandlerInvokeDate>();
            flowInvokeData.SetMaxSize(2000);

            listHandlers = Register(delegate(List<Handler> list) {
                list.Clear();
            });

            entityNode = Register(delegate(EntityNode e) {
                e.Clear();
            });

            entityNode.SetMaxSize(1000);
            eventBuilder = Register<EventBuilderImpl>();
            handlerExecutor = Register<HandlerExecutor>();
            handlerExecutor.SetMaxSize(1000);
        }

        public override void OnFlowClean() {
            base.OnFlowClean();
            array.FreeAll();
            Dictionary<Type, NodeInstanceCache>.Enumerator enumerator = nodeInstancesCache.GetEnumerator();

            while (enumerator.MoveNext()) {
                enumerator.Current.Value.OnFlowClean();
            }
        }

        public IList GetGenericListInstance(Type nodeClass, int count) {
            Type type = genericListInstances.ComputeIfAbsent(nodeClass, k => typeof(List<>).MakeGenericType(k));
            return (IList)Activator.CreateInstance(type, count);
        }

        public Node GetNodeInstance(Type nodeClass) {
            NodeInstanceCache value;

            if (!nodeInstancesCache.TryGetValue(nodeClass, out value)) {
                value = new NodeInstanceCache(nodeClass);
                nodeInstancesCache.Add(nodeClass, value);
            }

            return value.GetInstance();
        }

        public void FreeNodeInstance(Node node) {
            NodeInstanceCache value;

            if (nodeInstancesCache.TryGetValue(node.GetType(), out value)) {
                value.Free(node);
            }
        }
    }
}
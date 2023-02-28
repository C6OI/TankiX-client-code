using System.Collections.Generic;
using System.Diagnostics;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientLogger.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerArgumetCombinator {
        [Inject] public static FlowInstancesCache Cache { get; set; }

        public virtual bool Combine(HandlerInvokeGraph handlerInvokeGraph, ICollection<Entity> contextEntities) {
            ArgumentNode[] argumentNodes = handlerInvokeGraph.ArgumentNodes;
            ArgumentNode fromArgumentNode = null;
            Handler handler = handlerInvokeGraph.Handler;

            foreach (ArgumentNode argumentNode in argumentNodes) {
                if (!FillEntityNodes(contextEntities, fromArgumentNode, argumentNode)) {
                    if (handler.Mandatory || argumentNode.argument.Mandatory) {
                        throw new MandatoryException(SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, argumentNode, argumentNode.argument.JoinType));
                    }

                    return false;
                }

                fromArgumentNode = argumentNode;
            }

            Finalize(handlerInvokeGraph);
            return true;
        }

        bool FillEntityNodes(ICollection<Entity> contextEntities, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode) {
            HandlerArgument argument = toArgumentNode.argument;

            if (!toArgumentNode.filled) {
                Optional<JoinType> joinType = argument.JoinType;

                if (argument.Context && argument.SelectAll) {
                    if (contextEntities != null) {
                        FillEntityNodes(toArgumentNode, contextEntities);
                    } else {
                        FillEntityNodesBySelectAll(toArgumentNode);
                    }
                } else if (joinType.IsPresent()) {
                    if (joinType.Get() is JoinAllType) {
                        FillEntityNodesBySelectAll(toArgumentNode);
                    } else {
                        FillArgumentNodesByJoin(joinType.Get(), contextEntities, fromArgumentNode, toArgumentNode);
                    }
                }
            }

            return argument.Collection || argument.Optional || toArgumentNode.entityNodes.Count > 0;
        }

        void FillArgumentNodesByJoin(JoinType join, ICollection<Entity> contextEntities, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode) {
            for (int i = 0; i < fromArgumentNode.entityNodes.Count; i++) {
                FillEntityNodesByJoin(join, contextEntities, fromArgumentNode.entityNodes[i], toArgumentNode);
            }
        }

        public void FillEntityNodes(ArgumentNode argumentNode, ICollection<Entity> entities) {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Entity current = enumerator.Current;
                EntityNode entityNode;

                if (argumentNode.TryGetEntityNode(current, out entityNode)) {
                    argumentNode.entityNodes.Add(entityNode);
                }
            }
        }

        void FillEntityNodesBySelectAll(ArgumentNode argumentNode) {
            ICollection<Entity> entities = Flow.Current.NodeCollector.GetEntities(argumentNode.argument.NodeDescription);
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Entity current = enumerator.Current;
                EntityNode entityNode;

                if (argumentNode.TryGetEntityNode(current, out entityNode)) {
                    argumentNode.entityNodes.Add(entityNode);
                }
            }
        }

        void FillEntityNodesByJoin(JoinType join, ICollection<Entity> contextEntities, EntityNode fromEntityNode, ArgumentNode toArgumentNode) {
            ICollection<Entity> entities = join.GetEntities(Flow.Current.NodeCollector, toArgumentNode.argument.NodeDescription, fromEntityNode.entity);
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Entity current = enumerator.Current;
                EntityNode entityNode;

                if (!FilterByContext(toArgumentNode.argument, current, contextEntities) && toArgumentNode.TryGetEntityNode(current, out entityNode)) {
                    toArgumentNode.entityNodes.Add(entityNode);
                    fromEntityNode.nextArgumentEntityNodes.Add(entityNode);
                }
            }
        }

        bool FilterByContext(HandlerArgument argument, Entity entity, ICollection<Entity> contextEntities) =>
            argument.Context && contextEntities != null && !contextEntities.Contains(entity);

        void Finalize(HandlerInvokeGraph handlerInvokeGraph) {
            ArgumentNode[] argumentNodes = handlerInvokeGraph.ArgumentNodes;

            foreach (ArgumentNode argumentNode in argumentNodes) {
                argumentNode.PrepareInvokeArguments();

                if (argumentNode.argument.Collection) {
                    argumentNode.ConvertToCollection();
                } else if (argumentNode.argument.Optional) {
                    argumentNode.ConvertToOptional();
                }
            }
        }

        [Conditional("DEBUG")]
        void ShowDebugSkipInfo(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode) {
            if (handler.SkipInfo) {
                Optional<JoinType> join = fromArgumentNode == null ? Optional<JoinType>.empty() : fromArgumentNode.argument.JoinType;
                string skipReasonDetails = SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, toArgumentNode, join);
                LoggerProvider.GetLogger(this).Warn(skipReasonDetails);
            }
        }
    }
}
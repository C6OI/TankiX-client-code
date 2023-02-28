using System.Collections.Generic;
using System.Diagnostics;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientLogger.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeChangedReverseCombinator {
        [Inject] public static FlowInstancesCache Cache { get; set; }

        public bool Combine(HandlerInvokeGraph handlerInvokeGraph, Entity contextEntity, ICollection<NodeDescription> changedNodes) {
            ArgumentNode[] argumentNodes = handlerInvokeGraph.ArgumentNodes;
            ArgumentNode fromArgumentNode = null;

            for (int num = argumentNodes.Length - 1; num >= 0; num--) {
                ArgumentNode argumentNode = argumentNodes[num];

                if (!FillEntityNodes(fromArgumentNode, argumentNode, contextEntity, changedNodes)) {
                    Handler handler = handlerInvokeGraph.Handler;
                    return false;
                }

                fromArgumentNode = argumentNode;
            }

            return true;
        }

        protected bool FillEntityNodes(ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Entity contextEntity, ICollection<NodeDescription> changedNodes) {
            bool flag = toArgumentNode.argument.Context && changedNodes.Contains(toArgumentNode.argument.NodeDescription);

            if (fromArgumentNode != null && fromArgumentNode.filled) {
                Optional<JoinType> joinType;
                Optional<JoinType> optional = joinType = fromArgumentNode.argument.JoinType;

                if (optional.IsPresent() && !(joinType.Get() is JoinAllType)) {
                    FillArgumentNodesByJoin(joinType.Get(), fromArgumentNode, toArgumentNode, !flag ? null : contextEntity);
                    HandlerArgument argument = toArgumentNode.argument;

                    if (!argument.Collection && !argument.Optional && toArgumentNode.entityNodes.Count <= 0) {
                        return false;
                    }

                    goto IL_00c1;
                }
            }

            if (flag) {
                FillEntityNodes(toArgumentNode, contextEntity);
            }

            IL_00c1:
            return true;
        }

        public void FillEntityNodes(ArgumentNode argumentNode, Entity entity) {
            argumentNode.filled = true;
            EntityNode entityNode;

            if (argumentNode.TryGetEntityNode(entity, out entityNode)) {
                argumentNode.entityNodes.Add(entityNode);
            }
        }

        void FillArgumentNodesByJoin(JoinType join, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Entity contextEntity) {
            for (int i = 0; i < fromArgumentNode.entityNodes.Count; i++) {
                FillEntityNodesByJoin(join, fromArgumentNode.entityNodes[i], toArgumentNode, contextEntity);
            }
        }

        protected void FillEntityNodesByJoin(JoinType join, EntityNode fromEntityNode, ArgumentNode toArgumentNode, Entity contextEntity) {
            toArgumentNode.filled = true;
            ICollection<Entity> entities = join.GetEntities(Flow.Current.NodeCollector, toArgumentNode.argument.NodeDescription, fromEntityNode.entity);
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Entity current = enumerator.Current;
                EntityNode entityNode;

                if ((contextEntity == null || contextEntity.Equals(current)) && toArgumentNode.TryGetEntityNode(current, out entityNode)) {
                    entityNode.nextArgumentEntityNodes.Add(fromEntityNode);
                    toArgumentNode.entityNodes.Add(entityNode);
                }
            }
        }

        [Conditional("DEBUG")]
        void ShowDebugSkipInfo(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode) {
            if (handler.SkipInfo) {
                Optional<JoinType> joinType = toArgumentNode.argument.JoinType;
                string skipReasonDetails = SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, toArgumentNode, joinType);
                LoggerProvider.GetLogger(this).Warn(skipReasonDetails);
            }
        }
    }
}
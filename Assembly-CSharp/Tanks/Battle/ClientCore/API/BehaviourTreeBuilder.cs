using System;
using System.Collections;

namespace Tanks.Battle.ClientCore.API {
    public class BehaviourTreeBuilder {
        readonly Stack builderStack;

        BehaviourTreeNode root;

        readonly string treeName;

        public BehaviourTreeBuilder(string name) {
            builderStack = new Stack();
            treeName = name;
        }

        public BehaviourTreeBuilder Do(ActionNode action) {
            AddChild(action);
            return this;
        }

        public BehaviourTreeBuilder If(ConditionNode condition) {
            AddChild(condition);
            return this;
        }

        public BehaviourTreeBuilder ForTime(float time) {
            TimerNode timerNode = new();
            timerNode.Time = time;
            TimerNode timerNode2 = timerNode;
            AddChild(timerNode2);
            builderStack.Push(timerNode2);
            return this;
        }

        public BehaviourTreeBuilder StartDoOnceIn(float time) {
            OnceInTimeNode onceInTimeNode = new();
            onceInTimeNode.Time = time;
            OnceInTimeNode onceInTimeNode2 = onceInTimeNode;

            if (root == null) {
                root = onceInTimeNode2;
                builderStack.Push(onceInTimeNode2);
                return this;
            }

            AddChild(onceInTimeNode2);
            builderStack.Push(onceInTimeNode2);
            return this;
        }

        public BehaviourTreeBuilder ConnectTree(BehaviourTreeBuilder treePart) {
            BehaviourTreeNode child = treePart.Build();

            if (root == null) {
                root = child;
                return this;
            }

            AddChild(child);
            return this;
        }

        public BehaviourTreeBuilder StartSequence() {
            SequenceNode sequenceNode = new();

            if (root == null) {
                root = sequenceNode;
                builderStack.Push(sequenceNode);
                return this;
            }

            AddChild(sequenceNode);
            builderStack.Push(sequenceNode);
            return this;
        }

        public BehaviourTreeBuilder StartPreconditionSequence() {
            PreconditionSequence preconditionSequence = new();

            if (root == null) {
                root = preconditionSequence;
                builderStack.Push(preconditionSequence);
                return this;
            }

            AddChild(preconditionSequence);
            builderStack.Push(preconditionSequence);
            return this;
        }

        public BehaviourTreeBuilder StartSelector() {
            SelectorNode selectorNode = new();

            if (root == null) {
                root = selectorNode;
                builderStack.Push(selectorNode);
                return this;
            }

            AddChild(selectorNode);
            builderStack.Push(selectorNode);
            return this;
        }

        public BehaviourTreeBuilder StartParallel() {
            ParallelNode parallelNode = new();

            if (root == null) {
                root = parallelNode;
                builderStack.Push(parallelNode);
                return this;
            }

            AddChild(parallelNode);
            builderStack.Push(parallelNode);
            return this;
        }

        public BehaviourTreeBuilder End() {
            builderStack.Pop();
            return this;
        }

        public BehaviourTreeNode Build() {
            if (builderStack.Count != 0) {
                throw new Exception("One of composite nodes in tree wasn't closed! Tree name:" + treeName);
            }

            return root;
        }

        void AddChild(BehaviourTreeNode child) {
            object obj = builderStack.Peek();

            if (obj.GetType().BaseType == typeof(CompositeNode)) {
                (obj as CompositeNode).AddChild(child);
            }

            if (obj.GetType().BaseType == typeof(DecoratorNode)) {
                (obj as DecoratorNode).AddChild(child);
                builderStack.Pop();
            }
        }
    }
}
using System;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerArgument {
        public HandlerArgument(int nodeNumber, bool collection, NodeClassInstanceDescription nodeClassInstanceDescription,
            Optional<JoinType> joinJoinType, bool context, bool mandatory, bool combine, bool optional, Type argumentType) {
            NodeNumber = nodeNumber;
            Collection = collection;
            ClassInstanceDescription = nodeClassInstanceDescription;
            JoinType = joinJoinType;
            Context = context;
            Mandatory = mandatory;
            Combine = combine;
            Optional = optional;
            ArgumentType = argumentType;
            SelectAll = !JoinType.IsPresent() || JoinType.IsPresent() && JoinType.Get().GetType() == typeof(JoinAllType);
            Validate();
        }

        public int NodeNumber { get; internal set; }

        public bool Collection { get; internal set; }

        public NodeClassInstanceDescription ClassInstanceDescription { get; internal set; }

        public Optional<JoinType> JoinType { get; internal set; }

        public bool Context { get; internal set; }

        public bool Mandatory { get; internal set; }

        public bool Combine { get; internal set; }

        public bool Optional { get; internal set; }

        public Type ArgumentType { get; set; }

        public bool SelectAll { get; internal set; }

        public NodeDescription NodeDescription => ClassInstanceDescription.NodeDescription;

        void Validate() {
            if (Optional && Collection) {
                throw new OptionalCollectionNotValidException();
            }
        }

        public override string ToString() => string.Format("{0}[NodeNumber={1}, Collection={2}, ArgumentType={3}]",
            GetType(),
            NodeNumber,
            Collection,
            ArgumentType);
    }
}
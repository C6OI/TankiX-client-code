using System;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerArgumentBuilder {
        bool collection;

        bool combine;

        bool context;

        Optional<JoinType> joinType;

        bool mandatory;

        NodeClassInstanceDescription nodeClassInstanceDescription;

        bool optional;
        int position;

        Type type;

        public HandlerArgumentBuilder SetPosition(int position) {
            this.position = position;
            return this;
        }

        public HandlerArgumentBuilder SetCollection(bool collection) {
            this.collection = collection;
            return this;
        }

        public HandlerArgumentBuilder SetContext(bool context) {
            this.context = context;
            return this;
        }

        public HandlerArgumentBuilder SetNodeClassInstanceDescription(NodeClassInstanceDescription nodeClassInstanceDescription) {
            this.nodeClassInstanceDescription = nodeClassInstanceDescription;
            return this;
        }

        public HandlerArgumentBuilder SetJoinType(Optional<JoinType> joinType) {
            this.joinType = joinType;
            return this;
        }

        public HandlerArgumentBuilder SetMandatory(bool mandatory) {
            this.mandatory = mandatory;
            return this;
        }

        public HandlerArgumentBuilder SetCombine(bool combine) {
            this.combine = combine;
            return this;
        }

        public HandlerArgumentBuilder SetOptional(bool optional) {
            this.optional = optional;
            return this;
        }

        public HandlerArgumentBuilder SetType(Type type) {
            this.type = type;
            return this;
        }

        public HandlerArgument Build() => new(position, collection, nodeClassInstanceDescription, joinType, context, mandatory, combine, optional, type);
    }
}
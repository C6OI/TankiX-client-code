using System;

namespace YamlDotNet.Serialization.ObjectGraphVisitors {
    public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor {
        readonly IObjectGraphVisitor nextVisitor;

        protected ChainedObjectGraphVisitor(IObjectGraphVisitor nextVisitor) => this.nextVisitor = nextVisitor;

        public virtual bool Enter(IObjectDescriptor value) => nextVisitor.Enter(value);

        public virtual bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value) =>
            nextVisitor.EnterMapping(key, value);

        public virtual bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) =>
            nextVisitor.EnterMapping(key, value);

        public virtual void VisitScalar(IObjectDescriptor scalar) => nextVisitor.VisitScalar(scalar);

        public virtual void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType) =>
            nextVisitor.VisitMappingStart(mapping, keyType, valueType);

        public virtual void VisitMappingEnd(IObjectDescriptor mapping) => nextVisitor.VisitMappingEnd(mapping);

        public virtual void VisitSequenceStart(IObjectDescriptor sequence, Type elementType) =>
            nextVisitor.VisitSequenceStart(sequence, elementType);

        public virtual void VisitSequenceEnd(IObjectDescriptor sequence) => nextVisitor.VisitSequenceEnd(sequence);
    }
}
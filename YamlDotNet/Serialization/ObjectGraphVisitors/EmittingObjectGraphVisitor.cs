using System;

namespace YamlDotNet.Serialization.ObjectGraphVisitors {
    public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor {
        readonly IEventEmitter eventEmitter;

        public EmittingObjectGraphVisitor(IEventEmitter eventEmitter) => this.eventEmitter = eventEmitter;

        bool IObjectGraphVisitor.Enter(IObjectDescriptor value) => true;

        bool IObjectGraphVisitor.EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => true;

        bool IObjectGraphVisitor.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => true;

        void IObjectGraphVisitor.VisitScalar(IObjectDescriptor scalar) => eventEmitter.Emit(new ScalarEventInfo(scalar));

        void IObjectGraphVisitor.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType) =>
            eventEmitter.Emit(new MappingStartEventInfo(mapping));

        void IObjectGraphVisitor.VisitMappingEnd(IObjectDescriptor mapping) =>
            eventEmitter.Emit(new MappingEndEventInfo(mapping));

        void IObjectGraphVisitor.VisitSequenceStart(IObjectDescriptor sequence, Type elementType) =>
            eventEmitter.Emit(new SequenceStartEventInfo(sequence));

        void IObjectGraphVisitor.VisitSequenceEnd(IObjectDescriptor sequence) =>
            eventEmitter.Emit(new SequenceEndEventInfo(sequence));
    }
}
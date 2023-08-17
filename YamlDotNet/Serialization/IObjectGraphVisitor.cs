using System;

namespace YamlDotNet.Serialization {
    public interface IObjectGraphVisitor {
        bool Enter(IObjectDescriptor value);

        bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);

        bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);

        void VisitScalar(IObjectDescriptor scalar);

        void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType);

        void VisitMappingEnd(IObjectDescriptor mapping);

        void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);

        void VisitSequenceEnd(IObjectDescriptor sequence);
    }
}
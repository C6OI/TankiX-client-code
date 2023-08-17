using System;
using System.Collections.Generic;
using System.Globalization;

namespace YamlDotNet.Serialization.ObjectGraphVisitors {
    public sealed class AnchorAssigner : IAliasProvider, IObjectGraphVisitor {
        readonly IDictionary<object, AnchorAssignment> assignments = new Dictionary<object, AnchorAssignment>();

        uint nextId;

        string IAliasProvider.GetAlias(object target) {
            AnchorAssignment value;

            if (target != null && assignments.TryGetValue(target, out value)) {
                return value.Anchor;
            }

            return null;
        }

        bool IObjectGraphVisitor.Enter(IObjectDescriptor value) {
            if (value.Value == null || value.Type.GetTypeCode() != TypeCode.Object) {
                return false;
            }

            AnchorAssignment value2;

            if (assignments.TryGetValue(value.Value, out value2)) {
                if (value2.Anchor == null) {
                    value2.Anchor = "o" + nextId.ToString(CultureInfo.InvariantCulture);
                    nextId++;
                }

                return false;
            }

            assignments.Add(value.Value, new AnchorAssignment());
            return true;
        }

        bool IObjectGraphVisitor.EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => true;

        bool IObjectGraphVisitor.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => true;

        void IObjectGraphVisitor.VisitScalar(IObjectDescriptor scalar) { }

        void IObjectGraphVisitor.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType) { }

        void IObjectGraphVisitor.VisitMappingEnd(IObjectDescriptor mapping) { }

        void IObjectGraphVisitor.VisitSequenceStart(IObjectDescriptor sequence, Type elementType) { }

        void IObjectGraphVisitor.VisitSequenceEnd(IObjectDescriptor sequence) { }

        class AnchorAssignment {
            public string Anchor;
        }
    }
}
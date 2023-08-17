using System;
using System.Globalization;
using System.Linq;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies {
    public class RoundtripObjectGraphTraversalStrategy : FullObjectGraphTraversalStrategy {
        public RoundtripObjectGraphTraversalStrategy(Serializer serializer, ITypeInspector typeDescriptor,
            ITypeResolver typeResolver, int maxRecursion)
            : base(serializer, typeDescriptor, typeResolver, maxRecursion, null) { }

        protected override void TraverseProperties(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth) {
            if (!value.Type.HasDefaultConstructor() && !serializer.Converters.Any(c => c.Accepts(value.Type))) {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Type '{0}' cannot be deserialized because it does not have a default constructor or a type converter.",
                    value.Type));
            }

            base.TraverseProperties(value, visitor, currentDepth);
        }
    }
}
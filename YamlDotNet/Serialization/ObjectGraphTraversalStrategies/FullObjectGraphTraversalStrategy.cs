using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies {
    public class FullObjectGraphTraversalStrategy : IObjectGraphTraversalStrategy {
        static readonly GenericInstanceMethod<FullObjectGraphTraversalStrategy> traverseGenericDictionaryHelper =
            new(s => s.TraverseGenericDictionaryHelper<int, int>(null, null, 0, null));

        readonly int maxRecursion;

        readonly INamingConvention namingConvention;
        protected readonly Serializer serializer;

        readonly ITypeInspector typeDescriptor;

        readonly ITypeResolver typeResolver;

        public FullObjectGraphTraversalStrategy(Serializer serializer, ITypeInspector typeDescriptor,
            ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention) {
            if (maxRecursion <= 0) {
                throw new ArgumentOutOfRangeException("maxRecursion", maxRecursion, "maxRecursion must be greater than 1");
            }

            this.serializer = serializer;

            if (typeDescriptor == null) {
                throw new ArgumentNullException("typeDescriptor");
            }

            this.typeDescriptor = typeDescriptor;

            if (typeResolver == null) {
                throw new ArgumentNullException("typeResolver");
            }

            this.typeResolver = typeResolver;
            this.maxRecursion = maxRecursion;
            this.namingConvention = namingConvention;
        }

        void IObjectGraphTraversalStrategy.Traverse(IObjectDescriptor graph, IObjectGraphVisitor visitor) =>
            Traverse(graph, visitor, 0);

        protected virtual void Traverse(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth) {
            if (++currentDepth > maxRecursion) {
                throw new InvalidOperationException("Too much recursion when traversing the object graph");
            }

            if (!visitor.Enter(value)) {
                return;
            }

            TypeCode typeCode = value.Type.GetTypeCode();

            switch (typeCode) {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    visitor.VisitScalar(value);
                    return;

                case TypeCode.DBNull:
                    visitor.VisitScalar(new ObjectDescriptor(null, typeof(object), typeof(object)));
                    return;

                case TypeCode.Empty:
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                        "TypeCode.{0} is not supported.",
                        typeCode));
            }

            if (value.Value == null || value.Type == typeof(TimeSpan)) {
                visitor.VisitScalar(value);
                return;
            }

            Type underlyingType = Nullable.GetUnderlyingType(value.Type);

            if (underlyingType != null) {
                Traverse(new ObjectDescriptor(value.Value, underlyingType, value.Type, value.ScalarStyle),
                    visitor,
                    currentDepth);
            } else {
                TraverseObject(value, visitor, currentDepth);
            }
        }

        protected virtual void TraverseObject(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth) {
            if (typeof(IDictionary).IsAssignableFrom(value.Type)) {
                TraverseDictionary(value, visitor, currentDepth);
                return;
            }

            Type implementedGenericInterface =
                ReflectionUtility.GetImplementedGenericInterface(value.Type, typeof(IDictionary<,>));

            if (implementedGenericInterface != null) {
                TraverseGenericDictionary(value, implementedGenericInterface, visitor, currentDepth);
            } else if (typeof(IEnumerable).IsAssignableFrom(value.Type)) {
                TraverseList(value, visitor, currentDepth);
            } else {
                TraverseProperties(value, visitor, currentDepth);
            }
        }

        protected virtual void TraverseDictionary(IObjectDescriptor dictionary, IObjectGraphVisitor visitor,
            int currentDepth) {
            visitor.VisitMappingStart(dictionary, typeof(object), typeof(object));

            foreach (DictionaryEntry item in (IDictionary)dictionary.Value) {
                IObjectDescriptor objectDescriptor = GetObjectDescriptor(item.Key, typeof(object));
                IObjectDescriptor objectDescriptor2 = GetObjectDescriptor(item.Value, typeof(object));

                if (visitor.EnterMapping(objectDescriptor, objectDescriptor2)) {
                    Traverse(objectDescriptor, visitor, currentDepth);
                    Traverse(objectDescriptor2, visitor, currentDepth);
                }
            }

            visitor.VisitMappingEnd(dictionary);
        }

        void TraverseGenericDictionary(IObjectDescriptor dictionary, Type dictionaryType, IObjectGraphVisitor visitor,
            int currentDepth) {
            Type[] genericArguments = dictionaryType.GetGenericArguments();
            visitor.VisitMappingStart(dictionary, genericArguments[0], genericArguments[1]);

            traverseGenericDictionaryHelper.Invoke(genericArguments,
                this,
                dictionary.Value,
                visitor,
                currentDepth,
                namingConvention ?? new NullNamingConvention());

            visitor.VisitMappingEnd(dictionary);
        }

        void TraverseGenericDictionaryHelper<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IObjectGraphVisitor visitor,
            int currentDepth, INamingConvention namingConvention) {
            bool flag = dictionary.GetType().FullName.Equals("System.Dynamic.ExpandoObject");

            foreach (KeyValuePair<TKey, TValue> item in dictionary) {
                string value = !flag ? item.Key.ToString() : namingConvention.Apply(item.Key.ToString());
                IObjectDescriptor objectDescriptor = GetObjectDescriptor(value, typeof(TKey));
                IObjectDescriptor objectDescriptor2 = GetObjectDescriptor(item.Value, typeof(TValue));

                if (visitor.EnterMapping(objectDescriptor, objectDescriptor2)) {
                    Traverse(objectDescriptor, visitor, currentDepth);
                    Traverse(objectDescriptor2, visitor, currentDepth);
                }
            }
        }

        void TraverseList(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth) {
            Type implementedGenericInterface =
                ReflectionUtility.GetImplementedGenericInterface(value.Type, typeof(IEnumerable<>));

            Type type = implementedGenericInterface == null ? typeof(object)
                            : implementedGenericInterface.GetGenericArguments()[0];

            visitor.VisitSequenceStart(value, type);

            foreach (object item in (IEnumerable)value.Value) {
                Traverse(GetObjectDescriptor(item, type), visitor, currentDepth);
            }

            visitor.VisitSequenceEnd(value);
        }

        protected virtual void TraverseProperties(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth) {
            visitor.VisitMappingStart(value, typeof(string), typeof(object));

            foreach (IPropertyDescriptor property in typeDescriptor.GetProperties(value.Type, value.Value)) {
                IObjectDescriptor value2 = property.Read(value.Value);

                if (visitor.EnterMapping(property, value2)) {
                    Traverse(new ObjectDescriptor(property.Name, typeof(string), typeof(string)), visitor, currentDepth);
                    Traverse(value2, visitor, currentDepth);
                }
            }

            visitor.VisitMappingEnd(value);
        }

        IObjectDescriptor GetObjectDescriptor(object value, Type staticType) =>
            new ObjectDescriptor(value, typeResolver.Resolve(staticType, value), staticType);
    }
}
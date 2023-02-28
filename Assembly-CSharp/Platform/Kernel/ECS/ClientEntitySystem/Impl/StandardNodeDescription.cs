using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class StandardNodeDescription : AbstractNodeDescription {
        static readonly HashSet<Type> PlatformNodeClasses;

        static StandardNodeDescription() {
            PlatformNodeClasses = new HashSet<Type>();
            PlatformNodeClasses.Add(typeof(NewEntityNode));
            PlatformNodeClasses.Add(typeof(NotNewEntityNode));
            PlatformNodeClasses.Add(typeof(LoadedEntityNode));
            PlatformNodeClasses.Add(typeof(NotLoadedEntityNode));
            PlatformNodeClasses.Add(typeof(DeletedEntityNode));
            PlatformNodeClasses.Add(typeof(NotDeletedEntityNode));
            PlatformNodeClasses.Add(typeof(SharedEntityNode));
            PlatformNodeClasses.Add(typeof(NotSharedEntityNode));
            PlatformNodeClasses.Add(typeof(Node));
        }

        public StandardNodeDescription(Type nodeClass, ICollection<Type> additionalComponents = null)
            : base(CollectComponents(nodeClass), CollectNotComponents(nodeClass), additionalComponents) {
            Check(nodeClass);
        }

        static void Check(Type nodeClass) {
            if (!nodeClass.IsSubclassOf(typeof(AbstractSingleNode)) && nodeClass != typeof(Node)) {
                if (!nodeClass.IsNestedPublic) {
                    throw new NodeNotPublicException(nodeClass);
                }

                if (HasNonPublicComponents(nodeClass)) {
                    throw new NodeWithNonPublicComponentException(nodeClass);
                }
            }
        }

        static void CheckComponentName(Type nodeClass, Type fieldType, string fieldName) {
            string name = typeof(Component).Name;
            string text = "Behaviour";
            string name2 = fieldType.Name;

            if (char.ToLower(fieldName[0]) != fieldName[0]) {
                throw new WrongNodeFieldNameException(nodeClass, fieldType, fieldName);
            }

            if (name2.StartsWith(name) && fieldName == char.ToLower(name2[0]) + name2.Substring(1) || (fieldName + name).ToLower() == name2.ToLower() ||
                (fieldName + text).ToLower() == name2.ToLower() || fieldName == "marker") {
                return;
            }

            throw new WrongNodeFieldNameException(nodeClass, fieldType, fieldName);
        }

        static Type FindDeclaringSystemType(Type nodeClass) {
            Type declaringType = nodeClass.DeclaringType;

            while (declaringType != null && !typeof(ECSSystem).IsAssignableFrom(declaringType)) {
                declaringType = declaringType.DeclaringType;
            }

            return declaringType;
        }

        static bool HasNonPublicComponents(Type nodeClass) {
            IEnumerable<FieldInfo> fields = nodeClass.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (FieldInfo item in fields) {
                if (item.IsPublic || !IsComponent(item)) {
                    continue;
                }

                return true;
            }

            return false;
        }

        static ICollection<FieldInfo> CollectComponentsField(Type nodeClass) {
            ICollection<FieldInfo> collection = new HashSet<FieldInfo>();
            FieldInfo[] fields = nodeClass.GetFields();
            FieldInfo[] array = fields;

            foreach (FieldInfo fieldInfo in array) {
                if (IsComponent(fieldInfo)) {
                    collection.Add(fieldInfo);
                }
            }

            return collection;
        }

        static ICollection<Type> CollectComponents(Type nodeClass) {
            ICollection<Type> collection = new HashSet<Type>();
            FieldInfo[] fields = nodeClass.GetFields();
            FieldInfo[] array = fields;

            foreach (FieldInfo fieldInfo in array) {
                if (IsComponent(fieldInfo)) {
                    collection.Add(fieldInfo.FieldType);
                    continue;
                }

                throw new NodeFieldMustBeComponentTypeException(string.Format("Node: {0}, fieldName: {1}, fieldType: {2}", nodeClass.FullName, fieldInfo.Name, fieldInfo.FieldType));
            }

            return collection;
        }

        static ICollection<Type> CollectNotComponents(Type nodeClass) {
            ICollection<Type> collection = new HashSet<Type>();
            object[] customAttributes = nodeClass.GetCustomAttributes(typeof(Not), true);
            object[] array = customAttributes;

            for (int i = 0; i < array.Length; i++) {
                Not not = (Not)array[i];
                collection.Add(not.value);
            }

            return collection;
        }

        static bool IsComponent(FieldInfo fieldInfo) => typeof(Component).IsAssignableFrom(fieldInfo.FieldType);
    }
}
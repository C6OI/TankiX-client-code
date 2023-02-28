using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplateDescriptionImpl : TemplateDescription, HierarchyChangedListener {
        public const string TemplatePostfix = "Template";
        static readonly Type AutoAddedComponentInfoType = typeof(AutoAddedComponentInfo);

        readonly Dictionary<Type, ComponentDescription> componentDescriptionsByHierarchy;

        readonly ICollection<HierarchyChangedListener> hierarchyChangedListeners;

        readonly string overrideConfigPath;

        readonly IDictionary<Type, ComponentDescription> ownComponentDescriptions;

        readonly ICollection<TemplateDescriptionImpl> parentTemplateDescriptions;

        readonly PathOverrideType pathOverrideType;

        readonly Type templateClass;

        readonly long templateId;

        readonly string templateName;

        public readonly TemplateRegistry templateRegistry;

        ICollection<Type> autoAddedComponentTypes;

        volatile bool hierarchyChanged;

        public TemplateDescriptionImpl(TemplateRegistry templateRegistry, long templateId, Type templateClass) {
            this.templateRegistry = templateRegistry;
            ownComponentDescriptions = new Dictionary<Type, ComponentDescription>();
            componentDescriptionsByHierarchy = new Dictionary<Type, ComponentDescription>();
            hierarchyChangedListeners = new List<HierarchyChangedListener>();
            this.templateId = templateId;
            this.templateClass = templateClass;
            templateName = CreateTemplateName(templateClass);
            parentTemplateDescriptions = GetParentDescriptions(templateClass);
            overrideConfigPath = GetOverrideConfigPath(templateClass, out pathOverrideType);

            foreach (TemplateDescriptionImpl parentTemplateDescription in parentTemplateDescriptions) {
                parentTemplateDescription.AddHierarchyChangedListener(this);
            }
        }

        protected IDictionary<Type, ComponentDescription> ComponentDescriptionsByHierarchy {
            get {
                if (hierarchyChanged) {
                    componentDescriptionsByHierarchy.Clear();

                    foreach (KeyValuePair<Type, ComponentDescription> ownComponentDescription in ownComponentDescriptions) {
                        componentDescriptionsByHierarchy[ownComponentDescription.Key] = ownComponentDescription.Value;
                    }

                    foreach (TemplateDescriptionImpl parentTemplateDescription in parentTemplateDescriptions) {
                        foreach (KeyValuePair<Type, ComponentDescription> item in parentTemplateDescription.ComponentDescriptionsByHierarchy) {
                            componentDescriptionsByHierarchy[item.Key] = item.Value;
                        }
                    }

                    hierarchyChanged = false;
                }

                return componentDescriptionsByHierarchy;
            }
        }

        public void OnHierarchyChanged() {
            hierarchyChanged = true;

            foreach (HierarchyChangedListener hierarchyChangedListener in hierarchyChangedListeners) {
                hierarchyChangedListener.OnHierarchyChanged();
            }
        }

        public ICollection<ComponentDescription> ComponentDescriptions => ComponentDescriptionsByHierarchy.Values;

        public long TemplateId => templateId;

        public string TemplateName => templateName;

        public Type TemplateClass => templateClass;

        public string OverrideConfigPath(string path) {
            if (!IsOverridedConfigPath()) {
                return path;
            }

            if (pathOverrideType == PathOverrideType.RELATIVE) {
                return path + overrideConfigPath;
            }

            return overrideConfigPath;
        }

        public bool IsOverridedConfigPath() => !string.IsNullOrEmpty(overrideConfigPath);

        public bool IsComponentDescriptionPresent(Type componentClass) => ComponentDescriptionsByHierarchy.ContainsKey(componentClass);

        public ComponentDescription GetComponentDescription(Type componentClass) {
            ComponentDescription componentDescription = ComponentDescriptionsByHierarchy[componentClass];

            if (componentDescription == null) {
                throw new ComponentNotFoundInTemplateException(componentClass, templateName);
            }

            return componentDescription;
        }

        public ICollection<Type> GetAutoAddedComponentTypes() {
            if (autoAddedComponentTypes == null) {
                List<Type> list = new();

                foreach (ComponentDescription componentDescription in ComponentDescriptions) {
                    if (componentDescription.IsInfoPresent(AutoAddedComponentInfoType)) {
                        list.Add(componentDescription.ComponentType);
                    }
                }

                list.Sort((Type o1, Type o2) => o1.FullName.CompareTo(o2.FullName));
                autoAddedComponentTypes = list;
            }

            return autoAddedComponentTypes;
        }

        string GetOverrideConfigPath(Type template, out PathOverrideType pathOverrideType) {
            object[] customAttributes = template.GetCustomAttributes(false);

            foreach (object obj in customAttributes) {
                OverrideConfigPathAttribute overrideConfigPathAttribute = obj as OverrideConfigPathAttribute;

                if (overrideConfigPathAttribute != null) {
                    pathOverrideType = overrideConfigPathAttribute.pathOverrideType;
                    return overrideConfigPathAttribute.configPath;
                }
            }

            pathOverrideType = PathOverrideType.ABSOLUTE;
            return string.Empty;
        }

        public void AddComponentInfoFromClass(Type templateClass) {
            MethodInfo[] methods = templateClass.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo[] array = methods;

            foreach (MethodInfo methodInfo in array) {
                if (typeof(Component).IsAssignableFrom(methodInfo.ReturnType)) {
                    AddComponentInfoFromMethod(methodInfo);
                }
            }

            OnHierarchyChanged();
        }

        void AddComponentInfoFromMethod(MethodInfo method) {
            ComponentDescriptionImpl componentDescriptionImpl = new(method);
            componentDescriptionImpl.CollectInfo(templateRegistry.ComponentInfoBuilders);

            if (ownComponentDescriptions.ContainsKey(componentDescriptionImpl.ComponentType)) {
                throw new DuplicateComponentOnTemplateException(this, componentDescriptionImpl.ComponentType);
            }

            ownComponentDescriptions[componentDescriptionImpl.ComponentType] = componentDescriptionImpl;
        }

        public void AddHierarchyChangedListener(HierarchyChangedListener listener) {
            hierarchyChangedListeners.Add(listener);
        }

        ICollection<TemplateDescriptionImpl> GetParentDescriptions(Type templateClass) {
            ICollection<TemplateDescriptionImpl> collection = new List<TemplateDescriptionImpl>();
            ICollection<Type> parentTemplateClasses = templateRegistry.GetParentTemplateClasses(templateClass);

            foreach (Type item in parentTemplateClasses) {
                collection.Add((TemplateDescriptionImpl)templateRegistry.GetTemplateInfo(item));
            }

            return collection;
        }

        string CreateTemplateName(Type templateClass) {
            string text = templateClass.Name;

            if (text.EndsWith("Template", StringComparison.Ordinal)) {
                text = text.Substring(0, text.Length - "Template".Length);
            }

            return text;
        }
    }
}
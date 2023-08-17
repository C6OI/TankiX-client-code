using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplateRegistryImpl : TemplateRegistry {
        readonly IDictionary<long?, TemplateDescription> templates;

        public TemplateRegistryImpl() {
            templates = new Dictionary<long?, TemplateDescription>();
            ComponentInfoBuilders = new List<ComponentInfoBuilder>();
            RegisterCoreInfoBuilders();
        }

        public ICollection<ComponentInfoBuilder> ComponentInfoBuilders { get; }

        public TemplateDescription GetTemplateInfo(long id) {
            if (!templates.ContainsKey(id)) {
                throw new TemplateNotFoundException(id);
            }

            return templates[id];
        }

        public TemplateDescription GetTemplateInfo(Type templateClass) {
            long uid = SerializationUidUtils.GetUid(templateClass);
            return GetTemplateInfo(uid);
        }

        public void Register<T>() where T : Template => Register(typeof(T));

        public void Register(Type templateClass) {
            if (templateClass.IsDefined(typeof(TemplatePart), true)) {
                throw new CannotRegisterTemplatePartAsTemplateException(templateClass);
            }

            long uid = SerializationUidUtils.GetUid(templateClass);

            if (templates.ContainsKey(uid)) {
                return;
            }

            foreach (Type parentTemplateClass in GetParentTemplateClasses(templateClass)) {
                Register(parentTemplateClass);
            }

            TemplateDescriptionImpl templateDescriptionImpl = new(this, uid, templateClass);
            templateDescriptionImpl.AddComponentInfoFromClass(templateClass);
            templates[uid] = templateDescriptionImpl;
        }

        public void RegisterPart<T>() where T : Template => RegisterPart(typeof(T));

        public void RegisterPart(Type templatePartClass) {
            if (templatePartClass.GetCustomAttributes(typeof(TemplatePart), true).Length == 0) {
                throw new MissingTemplatePartAttributeException(templatePartClass);
            }

            ICollection<Type> directInterfaces = GetDirectInterfaces(templatePartClass);

            if (directInterfaces.Count != 1) {
                throw new TemplatePartShouldExtendSingleTemplateException(templatePartClass);
            }

            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator(directInterfaces);
            enumerator.MoveNext();
            Type current = enumerator.Current;

            if (!typeof(Template).IsAssignableFrom(current) || typeof(Template) == current) {
                throw new TemplatePartShouldExtendSingleTemplateException(templatePartClass, current);
            }

            Type templateClass = current;
            TemplateDescriptionImpl templateDescriptionImpl = (TemplateDescriptionImpl)GetTemplateInfo(templateClass);
            templateDescriptionImpl.AddComponentInfoFromClass(templatePartClass);
        }

        public void RegisterComponentInfoBuilder(ComponentInfoBuilder componentInfoBuilder) =>
            ComponentInfoBuilders.Add(componentInfoBuilder);

        public ICollection<Type> GetParentTemplateClasses(Type templateClass) {
            IList<Type> list = new List<Type>();
            Type[] interfaces = templateClass.GetInterfaces();

            foreach (Type type in interfaces) {
                if (typeof(Template).IsAssignableFrom(type) && type != typeof(Template)) {
                    list.Add(type);
                }
            }

            return list;
        }

        void RegisterCoreInfoBuilders() {
            RegisterComponentInfoBuilder(new AutoAddedComponentInfoBuilder());
            RegisterComponentInfoBuilder(new ConfigComponentInfoBuilder());
        }

        ICollection<Type> GetDirectInterfaces(Type interf) {
            HashSet<Type> hashSet = new();
            Type[] interfaces = interf.GetInterfaces();

            foreach (Type item in interfaces) {
                hashSet.Add(item);
            }

            HashSet<Type> hashSet2 = new();

            foreach (Type item3 in hashSet) {
                Type[] interfaces2 = item3.GetInterfaces();

                foreach (Type item2 in interfaces2) {
                    hashSet2.Add(item2);
                }
            }

            hashSet.ExceptWith(hashSet2);
            return hashSet;
        }
    }
}
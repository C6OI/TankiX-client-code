using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class EntityBuilder {
        static long idCounter = 4294967296L;

        protected readonly EngineServiceInternal engineServiceInternal;

        readonly EntityRegistry entityRegistry;

        protected readonly TemplateRegistry templateRegistry;

        string configPath;

        protected long? id;

        protected string name;

        Optional<TemplateAccessor> templateAccessor;

        TemplateDescription templateDescription;

        YamlNode templateYamlNode;

        public EntityBuilder(EngineServiceInternal engineServiceInternal, EntityRegistry entityRegistry,
            TemplateRegistry templateRegistry) {
            this.engineServiceInternal = engineServiceInternal;
            this.entityRegistry = entityRegistry;
            this.templateRegistry = templateRegistry;
        }

        public EntityBuilder SetId(long id) {
            this.id = id;
            return this;
        }

        public EntityBuilder MarkAsPersistent() => this;

        public EntityBuilder SetTemplateAccessor(Optional<TemplateAccessor> templateAccessor) {
            this.templateAccessor = templateAccessor;
            return this;
        }

        public EntityBuilder SetTemplate(Type templateType) {
            SetTemplate(templateRegistry.GetTemplateInfo(templateType));
            return this;
        }

        public EntityBuilder SetTemplate(TemplateDescription templateInfo) {
            templateDescription = templateInfo;
            return this;
        }

        public EntityBuilder SetTemplateYamlNode(YamlNode yamlNode) {
            templateYamlNode = yamlNode;
            return this;
        }

        public EntityBuilder SetName(string name) {
            this.name = name;
            return this;
        }

        public EntityBuilder SetConfig(string configPath) {
            this.configPath = configPath;
            return this;
        }

        public EntityInternal Build(bool registerInEngine = true) {
            long? num = id;

            if (!num.HasValue) {
                id = idCounter++;
            }

            if (name == null) {
                name = templateDescription == null ? Convert.ToString(id) : templateDescription.TemplateName;
            }

            if (templateAccessor == null) {
                templateAccessor = CreateTemplateAccessor();
            }

            EntityImpl entityImpl = CreateEntity(templateAccessor);

            if (registerInEngine) {
                entityRegistry.RegisterEntity(entityImpl);
                entityImpl.AddComponent(typeof(NewEntityComponent));
            }

            return entityImpl;
        }

        protected virtual EntityImpl CreateEntity(Optional<TemplateAccessor> templateAccessor) =>
            new(engineServiceInternal, id.Value, name, templateAccessor);

        Optional<TemplateAccessor> CreateTemplateAccessor() {
            if (templateDescription != null) {
                if (configPath == null) {
                    return Optional<TemplateAccessor>.of(new TemplateAccessor(templateDescription, templateYamlNode));
                }

                return Optional<TemplateAccessor>.of(new TemplateAccessor(templateDescription, configPath));
            }

            return Optional<TemplateAccessor>.empty();
        }
    }
}
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EntityShareCommand : AbstractCommand {
        EntityInternal _entity;

        [Inject] public static SharedEntityRegistry SharedEntityRegistry { get; set; }

        [ProtocolParameterOrder(0)] public long EntityId { get; set; }

        [ProtocolParameterOrder(1)] public Optional<TemplateAccessor> EntityTemplateAccessor { get; set; }

        [ProtocolParameterOrder(2)] [ProtocolCollection(false, true)]
        public Component[] Components { get; set; }

        [ProtocolTransient] public EntityInternal Entity {
            get => _entity;
            set {
                EntityId = value.Id;
                _entity = value;
            }
        }

        [ProtocolTransient] public string EntityName { get; set; }

        public override void Execute(Engine engine) {
            CreateEntity(engine);
        }

        void CreateEntity(Engine engine) {
            _entity = GetOrCreateEntity();
            _entity.Name = !string.IsNullOrEmpty(EntityName) ? string.Empty : GetNameFromTemplate();
            SharedEntityRegistry.SetShared(EntityId);

            Components.ForEach(delegate(Component c) {
                _entity.AddComponentSilent(c);
            });
        }

        public EntityInternal GetOrCreateEntity() {
            if (SharedEntityRegistry.TryGetEntity(EntityId, out _entity)) {
                _entity.TemplateAccessor = EntityTemplateAccessor;
            } else {
                _entity = SharedEntityRegistry.CreateEntity(EntityId, EntityTemplateAccessor);
            }

            return _entity;
        }

        string GetNameFromTemplate() {
            if (EntityTemplateAccessor.IsPresent()) {
                TemplateDescription templateDescription = EntityTemplateAccessor.Get().TemplateDescription;

                if (templateDescription != null) {
                    return templateDescription.TemplateName;
                }
            }

            return string.Empty;
        }

        public override string ToString() {
            string arg = EcsToStringUtil.EnumerableWithoutTypeToString(Components.Select(c => c.GetType().Name), ", ");
            return string.Format("EntityShareCommand: EntityId={0} Components={1}, Entity={2}", EntityId, arg, Entity);
        }
    }
}
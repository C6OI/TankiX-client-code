using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public class SharedEntityRegistryImpl : SharedEntityRegistry {
        readonly EngineServiceInternal engineService;

        readonly Dictionary<long, Pair<EntityState, EntityInternal>> registry = new();

        public SharedEntityRegistryImpl(EngineServiceInternal engineService) => this.engineService = engineService;

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public EntityInternal CreateEntity(long templateId, string configPath, long entityId) {
            EntityInternal entityInternal = engineService.CreateEntityBuilder().SetId(entityId).SetName(configPath)
                .SetConfig(configPath)
                .SetTemplate(TemplateRegistry.GetTemplateInfo(templateId))
                .Build(false);

            RegisterEntity(entityInternal, EntityState.Created);
            return entityInternal;
        }

        public EntityInternal CreateEntity(long entityId, Optional<TemplateAccessor> templateAccessor) {
            EntityInternal entityInternal = engineService.CreateEntityBuilder().SetId(entityId)
                .SetTemplateAccessor(templateAccessor)
                .Build(false);

            RegisterEntity(entityInternal, EntityState.Created);
            return entityInternal;
        }

        public EntityInternal CreateEntity(long entityId) {
            EntityInternal entityInternal = engineService.CreateEntityBuilder().SetId(entityId).Build(false);
            RegisterEntity(entityInternal, EntityState.Created);
            return entityInternal;
        }

        public bool TryGetEntity(long entityId, out EntityInternal entity) {
            entity = null;
            Pair<EntityState, EntityInternal> value;

            if (registry.TryGetValue(entityId, out value)) {
                entity = value.Value;
                return true;
            }

            return false;
        }

        public void SetShared(long entityId) {
            Pair<EntityState, EntityInternal> value;

            if (registry.TryGetValue(entityId, out value)) {
                if (value.Key == EntityState.Shared) {
                    throw new EntityAlreadySharedException(value.Value);
                }

                value.Value.Init();
                Flow.Current.EntityRegistry.RegisterEntity(value.Value);
                value.Key = EntityState.Shared;
                value.Value.AddComponent<SharedEntityComponent>();
                return;
            }

            throw new EntityByIdNotFoundException(entityId);
        }

        public void SetUnshared(long entityId) {
            Pair<EntityState, EntityInternal> value;

            if (registry.TryGetValue(entityId, out value) && value.Key == EntityState.Shared) {
                value.Key = EntityState.Unshared;
                return;
            }

            throw new EntityByIdNotFoundException(entityId);
        }

        public bool IsShared(long entityId) =>
            registry.ContainsKey(entityId) && registry[entityId].Key == EntityState.Shared;

        void RegisterEntity(EntityInternal entity, EntityState state) {
            if (registry.ContainsKey(entity.Id)) {
                throw new EntityAlreadyRegisteredException(entity);
            }

            registry.Add(entity.Id, new Pair<EntityState, EntityInternal>(EntityState.Created, entity));
        }

        enum EntityState {
            Created = 0,
            Shared = 1,
            Unshared = 2
        }
    }
}
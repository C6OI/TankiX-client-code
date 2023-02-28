using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public class SharedEntityRegistryImpl : SharedEntityRegistry {
        public static float CLEAN_PERIOD_SEC = 60f;

        public static float UNSHARE_EXPIRE_PERIOD_SEC = 60f;

        readonly EngineServiceInternal engineService;

        readonly List<long> entityToClean = new(100);

        double lastCleanTime;

        readonly Dictionary<long, EntityEntry> registry = new();

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
            EntityInternal entityInternal = engineService.CreateEntityBuilder().SetId(entityId).SetTemplateAccessor(templateAccessor)
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
            EntityEntry value;

            if (registry.TryGetValue(entityId, out value)) {
                entity = value.entity;
                return true;
            }

            return false;
        }

        public void SetShared(long entityId) {
            EntityEntry value;

            if (registry.TryGetValue(entityId, out value)) {
                if (value.state == EntityState.Shared) {
                    throw new EntityAlreadySharedException(value.entity);
                }

                value.entity.Init();
                Flow.Current.EntityRegistry.RegisterEntity(value.entity);
                value.state = EntityState.Shared;
                value.entity.AddComponent<SharedEntityComponent>();
                return;
            }

            throw new EntityByIdNotFoundException(entityId);
        }

        public void SetUnshared(long entityId) {
            EntityEntry value;

            if (registry.TryGetValue(entityId, out value) && value.state == EntityState.Shared) {
                value.state = EntityState.Unshared;
                value.unsharedTime = PreciseTime.Time;

                if (IsTimeToClean()) {
                    Clean();
                }

                return;
            }

            throw new EntityByIdNotFoundException(entityId);
        }

        public bool IsShared(long entityId) => registry.ContainsKey(entityId) && registry[entityId].state == EntityState.Shared;

        void RegisterEntity(EntityInternal entity, EntityState state) {
            if (registry.ContainsKey(entity.Id)) {
                throw new EntityAlreadyRegisteredException(entity);
            }

            registry.Add(entity.Id, new EntityEntry(EntityState.Created, entity));
        }

        bool IsTimeToClean() {
            if (PreciseTime.Time - lastCleanTime < CLEAN_PERIOD_SEC) {
                return false;
            }

            lastCleanTime = PreciseTime.Time;
            return true;
        }

        void Clean() {
            Dictionary<long, EntityEntry>.Enumerator enumerator = registry.GetEnumerator();

            while (enumerator.MoveNext()) {
                EntityEntry value = enumerator.Current.Value;

                if (value.IsEntryExpired()) {
                    entityToClean.Add(enumerator.Current.Key);
                }
            }

            for (int i = 0; i < entityToClean.Count; i++) {
                registry.Remove(entityToClean[i]);
            }

            entityToClean.Clear();
        }

        enum EntityState {
            Created = 0,
            Shared = 1,
            Unshared = 2
        }

        class EntityEntry {
            public readonly EntityInternal entity;

            public EntityState state;

            public double unsharedTime;

            public EntityEntry(EntityState state, EntityInternal entity) {
                this.entity = entity;
                this.state = state;
                unsharedTime = 0.0;
            }

            public bool IsEntryExpired() => state == EntityState.Unshared && PreciseTime.Time - unsharedTime > UNSHARE_EXPIRE_PERIOD_SEC;
        }
    }
}
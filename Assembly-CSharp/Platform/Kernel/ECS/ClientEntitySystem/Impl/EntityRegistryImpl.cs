using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityRegistryImpl : EntityRegistry {
        readonly IDictionary<long, Entity> entities;

        public EntityRegistryImpl() => entities = new Dictionary<long, Entity>();

        public Entity GetEntityForTest(string name) {
            foreach (Entity value in entities.Values) {
                if (name.Equals(value.Name)) {
                    return value;
                }
            }

            return null;
        }

        public ICollection<Entity> GetAllEntities() => entities.Values;

        public void Remove(long id) {
            if (!entities.Remove(id)) {
                throw new EntityByIdNotFoundException(id);
            }
        }

        public bool ContainsEntity(long id) => entities.ContainsKey(id);

        public void RegisterEntity(Entity entity) {
            try {
                entities.Add(entity.Id, entity);
            } catch (ArgumentException) {
                throw new EntityAlreadyRegisteredException(entity);
            }
        }

        public Entity GetEntity(long id) {
            try {
                return entities[id];
            } catch (KeyNotFoundException) {
                throw new EntityByIdNotFoundException(id);
            }
        }
    }
}
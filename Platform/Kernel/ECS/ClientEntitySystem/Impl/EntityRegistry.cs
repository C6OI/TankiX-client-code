using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EntityRegistry {
        Entity GetEntityForTest(string name);

        void Remove(long id);

        bool ContainsEntity(long id);

        void RegisterEntity(Entity entity);

        Entity GetEntity(long id);

        ICollection<Entity> GetAllEntities();
    }
}
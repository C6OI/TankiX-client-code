using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface SharedEntityRegistry {
        EntityInternal CreateEntity(long templateId, string configPath, long entityId);

        EntityInternal CreateEntity(long entityId, Optional<TemplateAccessor> templateAccessor);

        EntityInternal CreateEntity(long entityId);

        bool TryGetEntity(long entityId, out EntityInternal entity);

        void SetShared(long entityId);

        void SetUnshared(long entityId);

        bool IsShared(long entityId);
    }
}
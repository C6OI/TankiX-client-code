using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EntityTest : Entity {
        T GetComponentInTest<T>() where T : Component;

        bool HasComponentInTest<T>() where T : Component;

        void AddComponentInTest<RealT>(Component component) where RealT : Component;

        void UpdateNodes();
    }
}
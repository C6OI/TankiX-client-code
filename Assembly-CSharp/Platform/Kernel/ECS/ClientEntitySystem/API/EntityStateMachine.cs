using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EntityStateMachine {
        void AddState<T>() where T : Node, new();

        T ChangeState<T>() where T : Node;

        Node ChangeState(Type t);

        void AttachToEntity(Entity entity);
    }
}
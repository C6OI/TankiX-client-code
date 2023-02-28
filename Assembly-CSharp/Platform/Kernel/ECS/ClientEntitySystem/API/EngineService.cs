using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EngineService {
        Engine Engine { get; }

        TypeInstancesStorage<Event> EventInstancesStorageForReuse { get; }

        void RegisterTasksForHandler(Type handlerType);

        void RegisterHandlerFactory(HandlerFactory factory);

        void RegisterSystem(ECSSystem system);

        void AddSystemProcessingListener(EngineHandlerRegistrationListener listener);

        EntityBuilder CreateEntityBuilder();

        void AddFlowListener(FlowListener flowListener);

        void RemoveFlowListener(FlowListener flowListener);

        void AddComponentListener(ComponentListener componentListener);

        void AddEventListener(EventListener eventListener);

        void ExecuteInFlow(Consumer<Engine> consumer);
    }
}
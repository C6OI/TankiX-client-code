using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EngineService {
        Engine Engine { get; }

        event Consumer<Flow> FlowStartEvents;

        event Consumer<Flow> FlowFinishEvents;

        void RegisterTasksForHandler(Type handlerType);

        void RegisterHandlerFactory(HandlerFactory factory);

        void RegisterSystem(ECSSystem system);

        void AddSystemProcessingListener(EngineHandlerRegistrationListener listener);

        EntityBuilder CreateEntityBuilder();

        void ExecuteInFlow(Consumer<Engine> consumer);

        void AddFlowListener(FlowListener flowListener);

        void RemoveFlowListener(FlowListener flowListener);
    }
}
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EngineServiceInternal : EngineService {
        SystemRegistry SystemRegistry { get; }

        bool IsRunning { get; }

        HandlerCollector HandlerCollector { get; }

        EventMaker EventMaker { get; }

        BroadcastEventHandlerCollector BroadcastEventHandlerCollector { get; }

        HandlerStateListener HandlerStateListener { get; }

        DelayedEventManager DelayedEventManager { get; }

        Entity EntityStub { get; }

        ICollection<ComponentConstructor> ComponentConstructors { get; }

        EntityRegistry EntityRegistry { get; }

        NodeCollectorImpl NodeCollector { get; }

        ComponentBitIdRegistry ComponentBitIdRegistry { get; }

        NodeCache NodeCache { get; }

        HandlerContextDataStorage HandlerContextDataStorage { get; }

        ICollection<FlowListener> FlowListeners { get; }

        ICollection<ComponentListener> ComponentListeners { get; }

        ICollection<EventListener> EventListeners { get; }

        void RunECSKernel();

        Flow GetFlow();

        void RegisterComponentConstructor(ComponentConstructor componentConstructor);

        void RequireInitState();

        void ForceRegisterSystem(ECSSystem system);
    }
}
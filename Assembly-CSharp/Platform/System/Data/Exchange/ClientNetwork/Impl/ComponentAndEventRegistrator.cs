using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ComponentAndEventRegistrator : EngineHandlerRegistrationListener {
        readonly Protocol protocol;

        public ComponentAndEventRegistrator(EngineService engineService, Protocol protocol) {
            this.protocol = protocol;
            engineService.AddSystemProcessingListener(this);
        }

        public void OnHandlerAdded(Handler handler) {
            HandlerArgumentsDescription handlerArgumentsDescription = handler.HandlerArgumentsDescription;

            foreach (Type componentClass in handlerArgumentsDescription.ComponentClasses) {
                Register(componentClass);
            }

            foreach (Type eventClass in handlerArgumentsDescription.EventClasses) {
                Register(eventClass);
            }
        }

        public void Register(Type type) {
            if (IsShared(type)) {
                protocol.RegisterTypeWithSerialUid(type);
            }
        }

        public bool IsShared(Type type) => type.IsDefined(typeof(Shared), true);
    }
}
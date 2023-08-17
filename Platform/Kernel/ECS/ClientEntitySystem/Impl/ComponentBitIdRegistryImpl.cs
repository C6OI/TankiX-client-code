using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentBitIdRegistryImpl : TypeByIdRegistry, ComponentBitIdRegistry, EngineHandlerRegistrationListener {
        static long bitSequence;

        public ComponentBitIdRegistryImpl()
            : base(GetNextBitNumber) { }

        public int GetComponentBitId(Type componentClass) => (int)GetId(componentClass);

        public void OnHandlerAdded(Handler handler) => handler.HandlerArgumentsDescription.ComponentClasses.ForEach(
            delegate(Type t) {
                Register(t);
            });

        static long GetNextBitNumber(Type clazz) => ++bitSequence;
    }
}
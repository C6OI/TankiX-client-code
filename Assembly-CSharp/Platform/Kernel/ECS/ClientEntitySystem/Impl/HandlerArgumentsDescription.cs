using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerArgumentsDescription {
        public HandlerArgumentsDescription(IList<HandlerArgument> handlerArguments, ICollection<Type> additionalEventClasses, ICollection<Type> additionalComponentClasses) {
            HandlerArguments = handlerArguments;
            EventClasses = additionalEventClasses;
            ComponentClasses = additionalComponentClasses;
        }

        public IList<HandlerArgument> HandlerArguments { get; internal set; }

        public ICollection<Type> EventClasses { get; internal set; }

        public ICollection<Type> ComponentClasses { get; internal set; }
    }
}
using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class MandatoryException : Exception {
        public MandatoryException(ICollection<Entity> contexEntities, Handler handler)
            : base(new SkipLog(contexEntities, handler).ToString()) { }

        public MandatoryException(ICollection<Entity> contexEntities, Handler handler, HandlerArgument handlerArgument)
            : base(new SkipLog(contexEntities, handler).ToString()) { }

        public MandatoryException(string reason)
            : base(reason) { }
    }
}
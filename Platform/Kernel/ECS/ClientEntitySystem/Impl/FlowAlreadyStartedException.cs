using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FlowAlreadyStartedException : Exception {
        public FlowAlreadyStartedException(Flow flow)
            : base("out =" + flow) { }
    }
}
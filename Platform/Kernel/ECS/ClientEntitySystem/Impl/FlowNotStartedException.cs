using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FlowNotStartedException : Exception {
        public FlowNotStartedException(Flow flow)
            : base("flow =" + flow) { }
    }
}
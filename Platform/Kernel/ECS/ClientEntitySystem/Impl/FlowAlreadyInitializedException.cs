using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FlowAlreadyInitializedException : Exception {
        public FlowAlreadyInitializedException(Flow flow)
            : base("flow = " + flow) { }
    }
}
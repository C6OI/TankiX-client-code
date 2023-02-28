namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class FlowListenerAdapter : FlowListener {
        public static readonly FlowListenerAdapter Stub = new();

        public void OnFlowFinish() { }

        public void OnFlowClean() { }
    }
}
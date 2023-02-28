namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface FlowListener {
        void OnFlowFinish();

        void OnFlowClean();
    }
}
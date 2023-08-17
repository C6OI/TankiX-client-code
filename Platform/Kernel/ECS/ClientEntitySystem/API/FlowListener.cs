namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface FlowListener {
        void OnFlowSuccess();

        void OnFlowFatalError();

        void OnFlowClean();
    }
}
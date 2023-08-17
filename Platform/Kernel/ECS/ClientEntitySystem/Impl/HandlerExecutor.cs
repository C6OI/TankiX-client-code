namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerExecutor {
        public HandlerExecutor() { }

        public HandlerExecutor(Handler handler, object[] argumentForMethod) {
            Handler = handler;
            ArgumentForMethod = argumentForMethod;
        }

        public Handler Handler { get; private set; }

        public object[] ArgumentForMethod { get; private set; }

        public HandlerExecutor Init(Handler handler, object[] argumentForMethod) {
            Handler = handler;
            ArgumentForMethod = argumentForMethod;
            return this;
        }

        public void SetEvent(object eventInstance) => ArgumentForMethod[0] = eventInstance;

        public virtual void Execute() => Handler.Invoke(ArgumentForMethod);
    }
}
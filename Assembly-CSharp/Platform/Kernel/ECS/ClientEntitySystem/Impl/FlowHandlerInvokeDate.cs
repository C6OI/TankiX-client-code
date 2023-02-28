using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FlowHandlerInvokeDate : HandlerInvokeData {
        [Inject] public static FlowInstancesCache Cache { get; set; }

        protected override HandlerExecutor CreateExecutor() {
            object[] instanceArray = Cache.array.GetInstanceArray(HandlerArguments.Count + 1);
            return Cache.handlerExecutor.GetInstance().Init(Handler, instanceArray);
        }
    }
}
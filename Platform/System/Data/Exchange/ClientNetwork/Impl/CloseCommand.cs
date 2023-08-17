using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CloseCommand : Command {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public string Reason { get; set; }

        public void Execute(Engine engine) => EngineService.ExecuteInFlow(delegate(Engine e) {
            e.ScheduleEvent(new ServerConnectionCloseReasonEvent {
                    Reason = Reason
                },
                EngineService.EntityStub);
        });
    }
}
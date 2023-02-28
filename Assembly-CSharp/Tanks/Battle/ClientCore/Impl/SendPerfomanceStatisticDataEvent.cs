using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1470658766256L)]
    public class SendPerfomanceStatisticDataEvent : Event {
        public SendPerfomanceStatisticDataEvent(PerformanceStatisticData data) => Data = data;

        public PerformanceStatisticData Data { get; set; }
    }
}
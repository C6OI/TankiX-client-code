using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1470658766256L)]
    [Shared]
    public class SendPerfomanceStatisticDataEvent : Event {
        public SendPerfomanceStatisticDataEvent(PerformanceStatisticData data) => Data = data;

        public PerformanceStatisticData Data { get; set; }
    }
}
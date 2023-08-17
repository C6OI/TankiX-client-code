using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpdateItemPropertiesEvent : Event {
        public UpdateItemPropertiesEvent() { }

        public UpdateItemPropertiesEvent(long level, int proficiency) {
            Level = level;
            Proficiency = proficiency;
        }

        public long Level { get; set; }

        public int Proficiency { get; set; }
    }
}
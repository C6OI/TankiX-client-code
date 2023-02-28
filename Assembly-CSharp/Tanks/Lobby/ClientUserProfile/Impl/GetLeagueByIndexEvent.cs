using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class GetLeagueByIndexEvent : Event {
        public int index;

        public Entity leagueEntity;

        public GetLeagueByIndexEvent(int index) {
            this.index = index;
            leagueEntity = null;
        }
    }
}
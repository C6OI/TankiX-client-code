namespace Tanks.Lobby.ClientBattleSelect.API {
    public class TeamListUserData {
        public string hullIconId;

        public string hullName;

        public bool ready;

        public string turretIconId;

        public string turretName;
        public long userEntityId;

        public string userUid;

        public TeamListUserData(string userId) => userUid = userId;

        public TeamListUserData(long userEntityId, string userUid, bool ready) {
            this.userEntityId = userEntityId;
            this.userUid = userUid;
            this.ready = ready;
        }

        public TeamListUserData(long userEntityId, string userUid, string hullName, string hullIconId, string turretName, string turretIconId, bool ready) {
            this.userEntityId = userEntityId;
            this.userUid = userUid;
            this.hullName = hullName;
            this.hullIconId = hullIconId;
            this.turretName = turretName;
            this.turretIconId = turretIconId;
            this.ready = ready;
        }
    }
}
namespace Tanks.Lobby.ClientUserProfile.API {
    public class UserCellData {
        public long id;

        public string uid;

        public UserCellData(long id, string uid) {
            this.id = id;
            this.uid = uid;
        }
    }
}
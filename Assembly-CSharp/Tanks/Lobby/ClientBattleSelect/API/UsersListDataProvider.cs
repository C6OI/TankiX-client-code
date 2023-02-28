using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class UsersListDataProvider : DefaultListDataProvider {
        int maxCount;

        protected override void OnDisable() { }

        public TeamListUserData GetUserDataByUid(string uid) {
            foreach (object item in dataStorage) {
                TeamListUserData teamListUserData = item as TeamListUserData;

                if (teamListUserData != null && teamListUserData.userUid == uid) {
                    return teamListUserData;
                }
            }

            return null;
        }

        public void InitEmptyList(int maxCount) {
            this.maxCount = maxCount;

            for (int i = 0; i < maxCount; i++) {
                dataStorage.Add(null);
            }

            SendChanged();
        }

        public int GetUsersCount() {
            int num = 0;

            foreach (object item in dataStorage) {
                if (item != null) {
                    num++;
                }
            }

            return num;
        }

        int GetFirstNullIndex() {
            int num = 0;

            for (int i = 0; i < dataStorage.Count; i++) {
                if (dataStorage[i] != null) {
                    num++;
                    continue;
                }

                return num;
            }

            return -1;
        }

        public override void AddItem(object data) {
            int firstNullIndex = GetFirstNullIndex();

            if (firstNullIndex == -1) {
                dataStorage.Add(data);
            } else {
                dataStorage[firstNullIndex] = data;
            }

            SendChanged();
        }

        public override void RemoveItem(object data) {
            dataStorage.Remove(data);
            dataStorage.Add(null);
            SendChanged();
        }

        public virtual void UpdateItem(object oldData, object newData) {
            int num = dataStorage.IndexOf(oldData);

            if (num != -1) {
                dataStorage[num] = newData;
                SendChanged();
            }
        }
    }
}
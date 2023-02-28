using System.Collections.Generic;
using System.Linq;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CellsProgressBar : MonoBehaviour {
        public GameObject emptyCell;

        public GameObject filledCell;

        public GameObject filledEpicCell;

        void OnDisable() {
            transform.DestroyChildren();
        }

        public void Init(int capacity, DailyBonusData[] bonusData, List<long> receivedRewards) {
            transform.DestroyChildren();

            foreach (long receivedReward in receivedRewards) {
                DailyBonusData bonusData2 = getBonusData(receivedReward, bonusData);
                CreateFromPrefab(!bonusData2.IsEpic() ? filledCell : filledEpicCell);
            }

            int num = capacity - receivedRewards.Count;

            for (int i = 0; i < num; i++) {
                CreateFromPrefab(emptyCell);
            }
        }

        void CreateFromPrefab(GameObject prefab) {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.SetParent(transform, false);
        }

        DailyBonusData getBonusData(long receivedReward, DailyBonusData[] bonusData) {
            return bonusData.First(it => it.Code == receivedReward);
        }
    }
}
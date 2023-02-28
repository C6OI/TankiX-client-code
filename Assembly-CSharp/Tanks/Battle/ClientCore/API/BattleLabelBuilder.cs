using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class BattleLabelBuilder {
        public static GameObject battleLabelPrefab;

        readonly GameObject battleLabelInstance;

        public BattleLabelBuilder(long battleId) => battleLabelInstance = InstantiateBattleLabel(battleId);

        public GameObject Build() {
            battleLabelInstance.SetActive(true);
            return battleLabelInstance;
        }

        GameObject InstantiateBattleLabel(long battleId) {
            GameObject gameObject = Object.Instantiate(battleLabelPrefab);
            gameObject.SetActive(false);
            gameObject.GetComponent<BattleLabelComponent>().BattleId = battleId;
            return gameObject;
        }
    }
}
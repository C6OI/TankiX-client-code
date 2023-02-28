using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPMainStatComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI killsCount;

        [SerializeField] TextMeshProUGUI assistsCount;

        [SerializeField] TextMeshProUGUI deathsCount;

        [SerializeField] GameObject kills;

        [SerializeField] GameObject assists;

        [SerializeField] GameObject deaths;

        public void Set(UserResult mvp, BattleResultForClient battleResultForClient) {
            assists.SetActive(battleResultForClient.BattleMode != BattleMode.DM);
            killsCount.SetText(mvp.Kills.ToString());
            assistsCount.SetText(mvp.KillAssists.ToString());
            deathsCount.SetText(mvp.Deaths.ToString());
        }
    }
}
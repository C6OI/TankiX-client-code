using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [RequireComponent(typeof(ScrollRect))]
    public class BattleResultsScoreTableComponent : BehaviourComponent {
        public PlayerStatInfoUI rowPrefab;

        void OnDisable() {
            ScrollRect component = GetComponent<ScrollRect>();

            if (component.content != null) {
                component.content.DestroyChildren();
            }
        }
    }
}
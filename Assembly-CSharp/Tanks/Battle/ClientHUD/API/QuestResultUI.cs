using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.API {
    public class QuestResultUI : MonoBehaviour {
        [SerializeField] AnimatedDiffProgress progress;

        [SerializeField] TextMeshProUGUI task;

        [SerializeField] TextMeshProUGUI reward;
    }
}
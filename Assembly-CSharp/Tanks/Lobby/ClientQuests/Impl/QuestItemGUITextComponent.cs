using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestItemGUITextComponent : LocalizedControl {
        [SerializeField] TextMeshProUGUI progress;

        [SerializeField] TextMeshProUGUI pickUp;

        [SerializeField] TextMeshProUGUI nextQuest;

        public string Progress {
            set => progress.text = value;
        }

        public string PickUp {
            set => pickUp.text = value;
        }

        public string NextQuest {
            set => nextQuest.text = value;
        }
    }
}
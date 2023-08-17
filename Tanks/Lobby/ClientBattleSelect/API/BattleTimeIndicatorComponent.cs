using Lobby.ClientControls.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1447751145383L)]
    public class BattleTimeIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text timeText;

        [SerializeField] ProgressBar timeProgressBar;

        public string Time {
            get => timeText.text;
            set => timeText.text = value;
        }

        public float Progress {
            get => timeProgressBar.ProgressValue;
            set => timeProgressBar.ProgressValue = value;
        }
    }
}
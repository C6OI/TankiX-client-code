using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.API {
    [SerialVersionUID(635824353134005226L)]
    public class ErrorScreenComponent : MonoBehaviour, Component {
        [SerializeField] Text description;

        [SerializeField] Text buttonLabel;

        [SerializeField] Text reportButtonLabel;

        public virtual string Description {
            get => description.text;
            set => description.text = value;
        }

        public virtual string ButtonLabel {
            get => buttonLabel.text;
            set => buttonLabel.text = value;
        }

        public virtual string ReportButtonLabel {
            get => reportButtonLabel.text;
            set => reportButtonLabel.text = value;
        }
    }
}
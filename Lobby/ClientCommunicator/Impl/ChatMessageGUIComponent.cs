using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(635839577811878280L)]
    public class ChatMessageGUIComponent : MonoBehaviour, Component {
        [SerializeField] Text text;

        [SerializeField] Text username;

        [SerializeField] Text time;

        [SerializeField] Transform avatar;

        bool self;

        public string Username {
            get => username.text;
            set => username.text = value;
        }

        public string Text {
            get => text.text;
            set => text.text = value;
        }

        public string Time {
            get => time.text;
            set => time.text = value;
        }

        public bool Self {
            get => self;
            set {
                self = value;

                if (value) {
                    avatar.SetAsLastSibling();
                } else {
                    avatar.SetAsFirstSibling();
                }
            }
        }
    }
}
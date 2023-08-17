using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(635839578818657050L)]
    public class SectionListElementComponent : MonoBehaviour, Component {
        [SerializeField] GameObject sectionName;

        public Entity ChatDescriptionEntity { get; set; }

        public string Name {
            get => sectionName.GetComponent<Text>().text;
            set => sectionName.GetComponent<Text>().text = value;
        }
    }
}
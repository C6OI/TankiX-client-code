using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientCommunicator.Impl {
    [SerialVersionUID(635839578562486550L)]
    public class SectionContentGUIComponent : MonoBehaviour, Component {
        [SerializeField] GameObject sectionAsset;

        public GameObject SectionAsset => sectionAsset;
    }
}
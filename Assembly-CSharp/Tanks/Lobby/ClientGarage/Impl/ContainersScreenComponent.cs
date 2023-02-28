using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainersScreenComponent : BehaviourComponent {
        [SerializeField] GameObject openButton;

        [SerializeField] GameObject openAllButton;

        [SerializeField] GameObject rightPanel;

        [SerializeField] GameObject emptyListText;

        [SerializeField] GameObject contentButton;

        public GameObject OpenButton => openButton;

        public GameObject OpenAllButton => openAllButton;

        public bool OpenButtonActivity {
            get => openButton.activeSelf;
            set {
                openButton.SetActive(value);
                openButton.SetInteractable(value);
            }
        }

        public GameObject RightPanel => rightPanel;

        public GameObject EmptyListText => emptyListText;

        public bool ContentButtonActivity {
            get => contentButton.activeSelf;
            set {
                contentButton.SetActive(value);
                contentButton.SetInteractable(value);
            }
        }

        public void SetOpenButtonsActive(bool openActivity, bool openAllActivity) {
            openButton.SetActive(openActivity);
            openButton.SetInteractable(openActivity);
            openAllButton.SetActive(openAllActivity);
            openAllButton.SetInteractable(openAllActivity);
        }

        public void SetOpenButtonsInteractable(bool interactable) {
            openButton.SetInteractable(interactable);
            openAllButton.SetInteractable(interactable);
        }
    }
}
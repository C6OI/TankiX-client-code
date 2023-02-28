using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class GameModeSelectButtonComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] TextMeshProUGUI modeTitle;

        [SerializeField] TextMeshProUGUI modeDescription;

        [SerializeField] GameObject blockLayer;

        [SerializeField] GameObject restriction;

        [SerializeField] ImageSkin modeImage;

        [SerializeField] Material grayscaleMaterial;

        [SerializeField] GameObject notAvailableForNotSquadLeaderLabel;

        [SerializeField] GameObject notAvailableInSquadLabel;

        bool pointerInside;

        public string GameModeTitle {
            get => modeTitle.text;
            set => modeTitle.text = value;
        }

        public string ModeDescription {
            get => modeDescription.text;
            set => modeDescription.text = value;
        }

        public bool Restricted { get; private set; }

        void OnEnable() {
            SetAvailableForSquadMode(false);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            pointerInside = true;

            if (!TutorialCanvas.Instance.IsShow || GetComponent<Button>().interactable) {
                GetComponent<Animator>().SetTrigger("ShowDescription");
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            pointerInside = false;

            if (!TutorialCanvas.Instance.IsShow || GetComponent<Button>().interactable) {
                GetComponent<Animator>().SetTrigger("HideDescription");
            }
        }

        public void SetRestricted(bool restricted) {
            Restricted = restricted;
            restriction.gameObject.SetActive(restricted);
            blockLayer.gameObject.SetActive(restricted);
            CheckForTutorialEvent checkForTutorialEvent = new();
            ScheduleEvent(checkForTutorialEvent, new EntityStub());
            SetAvailableForSquadMode(false);

            if (!restricted && checkForTutorialEvent.TutorialIsActive) {
                GetComponent<Button>().interactable = false;
            } else {
                GetComponent<Button>().interactable = !restricted;
            }
        }

        public void SetAvailableForSquadMode(bool userInSquadNow, bool userIsSquadLeader = false, bool modeIsDefault = false) {
            notAvailableInSquadLabel.gameObject.SetActive(false);
            notAvailableForNotSquadLeaderLabel.gameObject.SetActive(false);

            if (!Restricted && userInSquadNow) {
                if (userIsSquadLeader && modeIsDefault) {
                    notAvailableInSquadLabel.gameObject.SetActive(true);
                } else if (!userIsSquadLeader) {
                    notAvailableForNotSquadLeaderLabel.gameObject.SetActive(true);
                }
            }
        }

        public void SetInactive() {
            Restricted = true;
            SetAvailableForSquadMode(true);
            blockLayer.gameObject.SetActive(true);
            GetComponent<Button>().interactable = false;
            modeImage.gameObject.GetComponent<Image>().material = grayscaleMaterial;
        }

        public void SetImage(string spriteUid) {
            modeImage.SpriteUid = spriteUid;
            modeImage.enabled = true;
        }
    }
}
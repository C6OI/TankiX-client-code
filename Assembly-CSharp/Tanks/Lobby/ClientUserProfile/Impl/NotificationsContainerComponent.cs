using System.Collections;
using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NotificationsContainerComponent : BehaviourComponent {
        [SerializeField] List<GameObject> rows;

        [SerializeField] GameObject fullSceenNotificationContainer;

        [SerializeField] int columnsCount;

        public int openedCards;

        public int hidenCards;

        [SerializeField] GameObject OpenAllCardsButton;

        [SerializeField] GameObject CloseAllCardsButton;

        [SerializeField] GameObject cardsCamera;

        [SerializeField] GameObject outlineBlurCamera;

        bool isHiden;

        public int MaxItemsPerScreen => rows.Count * columnsCount;

        public void Update() {
            int num = 0;

            for (int i = 0; i < rows.Count; i++) {
                for (int j = 0; j < rows[i].transform.childCount; j++) {
                    Transform child = rows[i].transform.GetChild(j);

                    if (child.GetComponentInChildren<NewCardNotificationComponent>() != null) {
                        num++;
                    }
                }
            }

            if (num == 0) {
                OpenAllCardsButton.SetActive(false);
                CloseAllCardsButton.SetActive(false);
                cardsCamera.SetActive(false);
                outlineBlurCamera.SetActive(false);
            }

            if (num > 0 && openedCards != num) {
                OpenAllCardsButton.SetActive(true);
                CloseAllCardsButton.SetActive(false);
                cardsCamera.SetActive(true);
                outlineBlurCamera.SetActive(true);
            }

            if (num > 0 && openedCards == num) {
                OpenAllCardsButton.SetActive(false);
                CloseAllCardsButton.SetActive(true);
                cardsCamera.SetActive(true);
                outlineBlurCamera.SetActive(true);
            }

            if (num == hidenCards && num > 0 && !isHiden) {
                isHiden = true;
                StartCoroutine(CloseHidenCards());
            }
        }

        public Transform GetParenTransform(int index, int count) => rows[GetRowIndex(index, count)].transform;

        public Transform GetFullSceenNotificationContainer() => fullSceenNotificationContainer.transform;

        int GetRowIndex(int index, int count) {
            int num = columnsCount;

            if (count == 4) {
                num = 2;
            }

            int num2 = index / num;

            if (num2 >= rows.Count) {
                num2 = rows.Count - 1;
            }

            return num2;
        }

        IEnumerator CloseHidenCards() {
            yield return new WaitForSeconds(0.3f);

            ExecuteEvents.Execute(eventData: new PointerEventData(EventSystem.current), target: CloseAllCardsButton.GetComponent<Button>().gameObject,
                functor: ExecuteEvents.submitHandler);

            yield return new WaitForSeconds(0.5f);

            isHiden = false;
        }
    }
}
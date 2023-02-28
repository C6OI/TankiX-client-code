using System.Collections;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NewCardNotificationComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, AttachToEntityListener, DetachFromEntityListener,
        IEventSystemHandler {
        [SerializeField] bool clickAnywhere;

        [SerializeField] Transform container;

        Entity entity;

        bool isClicked;

        bool isHiden;

        void Awake() {
            GetComponent<Animator>().SetFloat("multiple", Random.Range(0.9f, 1.1f));
        }

        void OnDestroy() {
            if (transform.GetComponentInParent<NotificationsContainerComponent>() != null && isClicked) {
                transform.GetComponentInParent<NotificationsContainerComponent>().openedCards--;

                if (isHiden) {
                    transform.GetComponentInParent<NotificationsContainerComponent>().hidenCards--;
                }
            }
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (isClicked && !isHiden) {
                GetComponent<Animator>().SetTrigger("end");
                transform.GetComponentInParent<NotificationsContainerComponent>().hidenCards++;
                isHiden = true;
            } else {
                MouseClicked();
                isClicked = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            GetComponent<Animator>().SetBool("selected", true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            GetComponent<Animator>().SetBool("selected", false);
        }

        public void OpenCardsButtonClicked() {
            GetComponent<Animator>().SetTrigger("Button");
            MouseClicked();
            isClicked = true;
        }

        public void CloseCardsButtonClicked() {
            GetComponent<Animator>().SetTrigger("end");
            StartCoroutine(NotificationClickEvent());
        }

        void MouseClicked() {
            if (!isClicked) {
                GetComponent<Animator>().SetTrigger("click");
                transform.GetComponentInParent<NotificationsContainerComponent>().openedCards++;
            }
        }

        IEnumerator NotificationClickEvent() {
            yield return new WaitForSeconds(0.5f);

            EngineService.Engine.ScheduleEvent<NotificationClickEvent>(entity);
            enabled = false;
        }

        void DestroyHidenCards() {
            StartCoroutine(NotificationClickEvent());
        }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UserListItemComponent : UIBehaviour, Component {
        public long userId;

        [SerializeField] GameObject userLabelPrefab;

        [SerializeField] RectTransform userLabelRoot;

        public bool isVisible;

        public RectTransform viewportRect;

        Animator animator;

        GameObject userLabelObject;

        [Inject] public static EngineService EngineService { get; set; }

        protected override void Awake() {
            animator = GetComponent<Animator>();
        }

        protected override void OnDisable() {
            CancelInvoke();
            animator.SetBool("show", false);
            animator.SetBool("remove", false);
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void ResetItem(long userId, bool delayedLoading = false) {
            CancelInvoke();
            this.userId = userId;
            isVisible = false;

            if (userLabelObject != null) {
                Destroy(userLabelObject);
            }

            animator.SetBool("show", false);
            animator.SetBool("remove", false);

            if (delayedLoading) {
                Invoke("SetUserLabelVisible", 0.2f);
            } else {
                SetUserLabelVisible();
            }
        }

        void SetUserLabelVisible() {
            isVisible = true;
            userLabelObject = Instantiate(userLabelPrefab);
            userLabelObject = new UserLabelBuilder(userId, userLabelObject, null, false).AllowInBattleIcon().Build();
            UidIndicatorComponent componentInChildren = userLabelObject.GetComponentInChildren<UidIndicatorComponent>();

            if (string.IsNullOrEmpty(componentInChildren.Uid)) {
                componentInChildren.OnUserUidInited.AddListener(UserInited);
            } else {
                UserInited();
            }

            Entity entity = userLabelObject.GetComponent<EntityBehaviour>().Entity;
            userLabelObject.GetComponent<RectTransform>().SetParent(userLabelRoot, false);

            userLabelObject.GetComponent<Button>().onClick.AddListener(delegate {
                EngineService.Engine.ScheduleEvent<UserLabelClickEvent>(entity);
            });
        }

        void UserInited() {
            userLabelObject.GetComponentInChildren<UidIndicatorComponent>().OnUserUidInited.RemoveListener(UserInited);
            GetComponent<Animator>().SetBool("show", true);
        }
    }
}
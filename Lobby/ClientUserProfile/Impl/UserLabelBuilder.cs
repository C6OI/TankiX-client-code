using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.Impl {
    public struct UserLabelBuilder {
        public static GameObject userLabelPrefab;

        readonly GameObject userLabelInstance;

        public UserLabelBuilder(long userId) {
            userLabelInstance = Object.Instantiate(userLabelPrefab);
            userLabelInstance.GetComponent<UserLabelComponent>().UserId = userId;
        }

        public UserLabelBuilder SkipLoadUserFromServer() {
            userLabelInstance.GetComponent<UserLabelComponent>().SkipLoadUserFromServer = true;
            return this;
        }

        public UserLabelBuilder WithoutAvatar() {
            userLabelInstance.GetComponentInChildren<UserLabelAvatarComponent>().gameObject.SetActive(false);
            return this;
        }

        public UserLabelBuilder SubscribeAvatarClick() {
            AddComponentToChildren<UserLabelAvatarComponent, UserLabelAvatarMappingComponent>(userLabelInstance);
            AddComponentToChildren<UserLabelAvatarComponent, CursorSwitcher>(userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeLevelClick() {
            AddComponentToChildren<RankIconComponent, UserLabelLevelMappingComponent>(userLabelInstance);
            AddComponentToChildren<RankIconComponent, CursorSwitcher>(userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeUidClick() {
            AddComponentToChildren<UidIndicatorComponent, UserLabelUidMappingComponent>(userLabelInstance);
            AddComponentToChildren<UidIndicatorComponent, CursorSwitcher>(userLabelInstance);
            return this;
        }

        public UserLabelBuilder SubscribeClick() {
            userLabelInstance.AddComponent<UserLabelMappingComponent>();
            return this;
        }

        public GameObject Build() {
            userLabelInstance.SetActive(true);
            Entity entity = userLabelInstance.GetComponent<EntityBehaviour>().Entity;
            UserLabelComponent component = userLabelInstance.GetComponent<UserLabelComponent>();

            if (component.SkipLoadUserFromServer) {
                entity.AddComponent(new UserGroupComponent(component.UserId));
            } else {
                entity.AddComponent(new LoadUserComponent(component.UserId));
            }

            return userLabelInstance;
        }

        static void AddComponentToChildren<M, C>(GameObject userLabel) where M : MonoBehaviour where C : MonoBehaviour {
            M componentInChildren = userLabel.GetComponentInChildren<M>();
            componentInChildren.gameObject.AddComponent<C>();
        }

        static void Unsubscribe<M, C>(GameObject userLabel)
            where M : MonoBehaviour where C : MonoBehaviour, Component, new() {
            M componentInChildren = userLabel.GetComponentInChildren<M>();
            C component = componentInChildren.gameObject.GetComponent<C>();
            Object.Destroy(component);
        }
    }
}
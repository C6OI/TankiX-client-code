using System;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public struct UserLabelBuilder {
        readonly GameObject userLabelInstance;

        public static GameObject userLabelPrefab;

        bool withLeague;

        public UserLabelBuilder(long userId, GameObject userLabelInstance, string avatarId, bool premium) {
            this.userLabelInstance = userLabelInstance;
            userLabelInstance.GetComponent<UserLabelComponent>().UserId = userId;

            if (!string.IsNullOrEmpty(avatarId)) {
                UserLabelAvatarComponent componentInChildren = userLabelInstance.GetComponentInChildren<UserLabelAvatarComponent>();
                componentInChildren.AvatarImage.SpriteUid = avatarId;
                componentInChildren.IsPremium = premium;
            }

            withLeague = false;
        }

        public static GameObject CreateDefaultLabel() {
            if (userLabelPrefab == null) {
                throw new Exception("User label prefab not found");
            }

            return Object.Instantiate(userLabelPrefab);
        }

        public UserLabelBuilder SetLeague(int league) {
            userLabelInstance.GetComponentInChildren<LeagueBorderComponent>().ImageListSkin.SelectedSpriteIndex = league;
            withLeague = true;
            return this;
        }

        public UserLabelBuilder SkipLoadUserFromServer() {
            userLabelInstance.GetComponent<UserLabelComponent>().SkipLoadUserFromServer = true;
            return this;
        }

        public UserLabelBuilder WithoutAvatar() {
            userLabelInstance.GetComponentInChildren<UserLabelAvatarComponent>().gameObject.SetActive(false);
            return this;
        }

        public UserLabelBuilder AllowInBattleIcon() {
            userLabelInstance.GetComponent<UserLabelComponent>().AllowInBattleIcon = true;
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
            AddComponentToChildren<UserLabelAvatarComponent, CursorSwitcher>(userLabelInstance);
            AddComponentToChildren<RankIconComponent, CursorSwitcher>(userLabelInstance);
            AddComponentToChildren<UidIndicatorComponent, CursorSwitcher>(userLabelInstance);
            return this;
        }

        public GameObject Build() {
            userLabelInstance.SetActive(true);
            Entity entity = userLabelInstance.GetComponent<EntityBehaviour>().Entity;
            UserLabelComponent component = userLabelInstance.GetComponent<UserLabelComponent>();

            if (component.SkipLoadUserFromServer) {
                if (!entity.HasComponent<UserGroupComponent>()) {
                    entity.AddComponent(new UserGroupComponent(component.UserId));
                } else if (entity.GetComponent<UserGroupComponent>().Key != component.UserId) {
                    entity.RemoveComponent<UserGroupComponent>();
                    entity.AddComponent(new UserGroupComponent(component.UserId));
                }
            } else {
                entity.AddComponent(new LoadUserComponent(component.UserId));
            }

            LeagueBorderComponent componentInChildren = userLabelInstance.GetComponentInChildren<LeagueBorderComponent>();

            if (componentInChildren != null) {
                componentInChildren.gameObject.SetActive(withLeague);
            }

            return userLabelInstance;
        }

        static void AddComponentToChildren<M, C>(GameObject userLabel) where M : MonoBehaviour where C : MonoBehaviour {
            M componentInChildren = userLabel.GetComponentInChildren<M>();
            GameObject gameObject = componentInChildren.gameObject;

            if (gameObject.GetComponent<C>() == null) {
                gameObject.AddComponent<C>();
            }
        }

        static void Unsubscribe<M, C>(GameObject userLabel) where M : MonoBehaviour where C : MonoBehaviour, Component, new() {
            M componentInChildren = userLabel.GetComponentInChildren<M>();
            C component = componentInChildren.gameObject.GetComponent<C>();
            Object.Destroy(component);
        }
    }
}
using System.Collections;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadColorTestSystem : ECSSystem {
        [OnEventFire]
        public void InitTest(NodeAddedEvent e, SelfSquadUser user) {
            user.Entity.AddComponent(new TeamColorComponent {
                TeamColor = TeamColor.BLUE
            });

            int x = -6;
            CreateUserCell(user.squadGroup.Key, x, -2, TeamColor.BLUE);
            CreateUserCell(user.squadGroup.Key, x, -3, TeamColor.BLUE);

            for (int i = 0; i < 10; i++) {
                TeamColor teamColor = i >= 4 ? TeamColor.RED : TeamColor.BLUE;
                long squadGroupKey = (long)Random.Range(0f, 1.2512351E+10f);
                x = i - 5;
                CreateUserCell(squadGroupKey, x, -2, teamColor);
                CreateUserCell(squadGroupKey, x, -3, teamColor);
            }
        }

        void CreateUserCell(long squadGroupKey, int x, int y, TeamColor teamColor) {
            GameObject gameObject = GameObject.Find("MainScreen");
            GameObject gameObject2 = new("User with squad group " + squadGroupKey);
            gameObject2.SetActive(false);
            gameObject2.transform.SetParent(gameObject.transform, false);
            GameObject gameObject3 = new("Color image");
            gameObject3.transform.SetParent(gameObject2.transform, false);
            UserSquadColorComponent userSquadColorComponent = gameObject2.AddComponent<UserSquadColorComponent>();
            userSquadColorComponent.Image = gameObject3.AddComponent<Image>();
            RectTransform component = gameObject3.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(20f, 20f);
            component.anchoredPosition = new Vector2(x * 30f, y * 30f);
            gameObject2.AddComponent<EntityBehaviour>().handleAutomaticaly = true;
            gameObject2.SetActive(true);
            gameObject2.GetComponent<MonoBehaviour>().StartCoroutine(AddGruops(gameObject2, squadGroupKey, teamColor));
        }

        IEnumerator AddGruops(GameObject go, long squadGroupKey, TeamColor teamColor) {
            yield return new WaitForSeconds(1f);

            Entity entity = go.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent(new UserGroupComponent(entity.Id));
            entity.AddComponent(new SquadGroupComponent(squadGroupKey));

            entity.AddComponent(new TeamColorComponent {
                TeamColor = teamColor
            });
        }

        public class SelfSquadUser : Node {
            public SelfUserComponent selfUser;

            public SquadGroupComponent squadGroup;
            public UserGroupComponent userGroup;
        }
    }
}
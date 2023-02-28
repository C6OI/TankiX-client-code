using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635883805937045890L)]
    public class ScoreTableUserLabelIndicatorComponent : MonoBehaviour, Component {
        public GameObject userLabel;

        public void Awake() {
            userLabel = UserLabelBuilder.CreateDefaultLabel();
            userLabel.transform.SetParent(gameObject.transform, false);
        }
    }
}
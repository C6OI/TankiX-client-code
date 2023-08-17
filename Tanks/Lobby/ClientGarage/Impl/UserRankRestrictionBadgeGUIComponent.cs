using Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserRankRestrictionBadgeGUIComponent : MonoBehaviour, Component {
        [SerializeField] ImageListSkin imageListSkin;

        public void SetRank(int rank) => imageListSkin.SelectSprite(rank.ToString());
    }
}
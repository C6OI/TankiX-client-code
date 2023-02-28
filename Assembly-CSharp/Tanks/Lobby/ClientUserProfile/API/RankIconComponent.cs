using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class RankIconComponent : MonoBehaviour, Component {
        [SerializeField] ImageListSkin imageListSkin;

        public ImageListSkin ImageListSkin => imageListSkin;

        public void SetRank(int rank) {
            imageListSkin.gameObject.SetActive(true);
            imageListSkin.SelectSprite(rank.ToString());
        }
    }
}
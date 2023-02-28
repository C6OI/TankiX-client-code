using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class LeagueBorderComponent : BehaviourComponent {
        [SerializeField] ImageListSkin imageListSkin;

        public ImageListSkin ImageListSkin => imageListSkin;

        public void SetLeague(int league) {
            imageListSkin.gameObject.SetActive(true);
            imageListSkin.SelectSprite(league.ToString());
        }
    }
}
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueRewardListUIComponent : MonoBehaviour {
        [SerializeField] LeagueRewardItem itemPrefab;

        public void Clear() {
            transform.DestroyChildren();
        }

        public void AddItem(string text, string imageUID) {
            GetNewItem(text, imageUID);
        }

        LeagueRewardItem GetNewItem(string text, string imageUID) {
            LeagueRewardItem leagueRewardItem = Instantiate(itemPrefab);
            leagueRewardItem.transform.SetParent(transform, false);
            leagueRewardItem.Text = text;
            leagueRewardItem.Icon = imageUID;
            leagueRewardItem.gameObject.SetActive(true);
            return leagueRewardItem;
        }
    }
}
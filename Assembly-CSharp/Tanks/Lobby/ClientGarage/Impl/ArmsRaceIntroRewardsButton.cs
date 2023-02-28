using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ArmsRaceIntroRewardsButton : MonoBehaviour {
        public Animator currentScreenAnimator;

        public GameObject rewardsScreen;

        public void ShowRewardsButtonClick() {
            currentScreenAnimator.SetTrigger("Hide");
            rewardsScreen.SetActive(true);
        }
    }
}
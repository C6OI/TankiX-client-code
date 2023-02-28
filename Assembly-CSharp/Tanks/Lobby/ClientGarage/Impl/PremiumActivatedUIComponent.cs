using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumActivatedUIComponent : ConfirmDialogComponent {
        [SerializeField] GameObject premIcon;

        [SerializeField] GameObject xpremIcon;

        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI reason;

        [SerializeField] TextMeshProUGUI days;

        [SerializeField] LocalizedField promoPremTitle;

        [SerializeField] LocalizedField promoPremText;

        [SerializeField] LocalizedField usualPremTitle;

        [SerializeField] LocalizedField premDays;

        [SerializeField] LocalizedField dayLocalizationCases;

        public void ShowPrem(List<Animator> animators, bool premWithQuest, int daysCount, bool promo = false) {
            premIcon.SetActive(!premWithQuest);
            xpremIcon.SetActive(premWithQuest);
            days.text = string.Format(premDays, daysCount, CasesUtil.GetLocalizedCase(dayLocalizationCases, daysCount));

            if (promo) {
                title.text = promoPremTitle;
                reason.text = string.Format(promoPremText, SelfUserComponent.SelfUser.GetComponent<UserUidComponent>().Uid);
                reason.gameObject.SetActive(true);
            } else {
                title.text = usualPremTitle;
            }

            Show(animators);
        }
    }
}
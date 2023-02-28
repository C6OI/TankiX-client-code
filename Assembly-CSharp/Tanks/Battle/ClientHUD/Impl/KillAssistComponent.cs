using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientControls.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class KillAssistComponent : BehaviourComponent {
        public CombatEventLog combatEventLog;

        public AnimatedLong scoreTotalNumber;

        public Animator totalNumberAnimator;

        public LocalizedField flagDeliveryMessage;

        public LocalizedField flagReturnMessage;

        public LocalizedField killMessage;

        public LocalizedField assistMessage;

        public LocalizedField healMessage;

        public LocalizedField streakMessage;

        bool visible;

        void Awake() {
            scoreTotalNumber.Value = 0L;
        }

        public void Clear() {
            SetTotalNumberToZero();
            combatEventLog.Clear();
        }

        public void AddKillStreakMessage(int score) {
            IncreaseTotalScore(score);
            string messageText = streakMessage.Value.Replace("{scoreValule}", score.ToString());
            combatEventLog.AddMessage(messageText);
        }

        public void AddFlagDeliveryMessage(int score) {
            IncreaseTotalScore(score);
            string messageText = flagDeliveryMessage.Value.Replace("{scoreValule}", score.ToString());
            combatEventLog.AddMessage(messageText);
        }

        public void AddFlagReturnMessage(int score) {
            IncreaseTotalScore(score);
            string messageText = flagReturnMessage.Value.Replace("{scoreValule}", score.ToString());
            combatEventLog.AddMessage(messageText);
        }

        public void AddAssistMessage(int score, int percent, string nickname) {
            IncreaseTotalScore(score);
            string value = assistMessage.Value;
            value = value.Replace("{scoreValule}", score.ToString());
            value = value.Replace("{percent}", percent.ToString());
            value = value.Replace("{killer}", ParseNickname(nickname));
            combatEventLog.AddMessage(value);
        }

        public void AddKillMessage(int score, string nickname, int rank) {
            IncreaseTotalScore(score);
            string value = killMessage.Value;
            value = value.Replace("{scoreValule}", score.ToString());
            value = CombatEventLogUtil.ApplyPlaceholder(value, "{killer}", rank, ParseNickname(nickname), Color.white);
            combatEventLog.AddMessage(value);
        }

        public void AddHealMessage(int score) {
            IncreaseTotalScore(score);
            string value = healMessage.Value;
            value = value.Replace("{scoreValule}", score.ToString());
            combatEventLog.AddMessage(value);
        }

        void IncreaseTotalScore(int score) {
            scoreTotalNumber.Value += score;
            totalNumberAnimator.SetBool("Visible", visible);
            totalNumberAnimator.SetTrigger("Show");
        }

        public void SetVisible(bool visible) {
            this.visible = visible;
        }

        string ParseNickname(string nickname) => nickname.Replace("botxz_", string.Empty);

        public void SetTotalNumberToZero() {
            scoreTotalNumber.Value = 0L;
        }

        public void ShowRandomAssistMessage() {
            string[] array = new string[5] { "Deathcraft", "OMOEWAMOE_SHIRANEU", "devochka", "kit", "Nagib-na-smoke" };
            int rank = Random.Range(1, 101);
            string nickname = array[Random.Range(0, array.Length)];

            switch (Random.Range(0, 4)) {
                case 0:
                    AddKillMessage(Random.Range(20, 40), nickname, rank);
                    break;

                case 1:
                    AddAssistMessage(Random.Range(1, 20), Random.Range(10, 50), nickname);
                    break;

                case 2:
                    AddFlagDeliveryMessage(Random.Range(20, 40));
                    break;

                default:
                    AddFlagReturnMessage(Random.Range(20, 40));
                    break;
            }
        }
    }
}
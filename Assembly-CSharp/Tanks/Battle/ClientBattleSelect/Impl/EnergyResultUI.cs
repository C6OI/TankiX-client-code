using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class EnergyResultUI : MonoBehaviour {
        [SerializeField] List<AnimatedDiffProgress> charges;

        [SerializeField] protected TextMeshProUGUI earnedEnergyText;

        [SerializeField] [FormerlySerializedAs("compensationText")]
        protected TextMeshProUGUI compensationCrystalsText;

        [SerializeField] [FormerlySerializedAs("compensationObject")]
        protected GameObject compensationCrystalsObject;

        [SerializeField] [FormerlySerializedAs("cashbackText")]
        protected TextMeshProUGUI mvpCashbackTextObject;

        [SerializeField] protected TextMeshProUGUI chargesFullText;

        [SerializeField] TooltipShowBehaviour energyBarTooltip;

        [SerializeField] TooltipShowBehaviour mvpBonusTooltip;

        [SerializeField] TooltipShowBehaviour unfairBonusTooltip;

        [SerializeField] [FormerlySerializedAs("earnedTextTemplate")]
        LocalizedField cashbackText;

        [SerializeField] [FormerlySerializedAs("cashbackEnergyTextTemplate")]
        LocalizedField mvpCashbackText;

        [SerializeField] LocalizedField unfairMatchText;

        [SerializeField] float duration = 0.3f;

        [SerializeField] Color fullColor = new Color32(132, 246, 246, byte.MaxValue);

        [SerializeField] Color partColor = new Color32(128, 128, 128, byte.MaxValue);

        long currentEnergy;

        int earnedEnergy;

        long energyInCharge;

        List<float> previousProgress;

        public void SetEnergyResult(long currentEnergy, long energyInCharge, int earnedEnergy) {
            previousProgress = new List<float>(new float[charges.Count]);
            long num = currentEnergy / energyInCharge;
            this.currentEnergy = currentEnergy;
            this.energyInCharge = energyInCharge;
            this.earnedEnergy = earnedEnergy;
            long num2 = currentEnergy - earnedEnergy;

            for (int i = 0; i < charges.Count; i++) {
                float num3 = Mathf.Clamp01((num2 - i * energyInCharge) / (float)energyInCharge);
                previousProgress[i] = num3;
                charges[i].Set(num3, num3);

                if (i < num) {
                    charges[i].FillImage.color = fullColor;
                    charges[i].DiffImage.color = fullColor;
                } else if (i == num) {
                    charges[i].FillImage.color = partColor;
                    charges[i].DiffImage.color = partColor;
                }
            }

            energyBarTooltip.TipText = string.Format("{0} / {1}", currentEnergy, energyInCharge * charges.Count);
        }

        public void SetEnergyCompensation(int compensationCrystals, bool mvpCashback, bool isUnfairCashback) {
            if (compensationCrystals > 0) {
                earnedEnergyText.gameObject.SetActive(false);
                compensationCrystalsText.text = string.Format(cashbackText, compensationCrystals);
                compensationCrystalsObject.SetActive(true);
                mvpCashbackTextObject.gameObject.SetActive(mvpCashback);
                chargesFullText.gameObject.SetActive(true);
            } else {
                earnedEnergyText.text = string.Format(!mvpCashback ? cashbackText : mvpCashbackText, earnedEnergy);
                earnedEnergyText.gameObject.SetActive(earnedEnergy > 0);
                compensationCrystalsObject.SetActive(false);
                chargesFullText.gameObject.SetActive(false);
            }

            mvpBonusTooltip.gameObject.SetActive(mvpCashback);
            unfairBonusTooltip.gameObject.SetActive(isUnfairCashback);
        }

        public void ShowEnergyResult() {
            int num = (int)((currentEnergy - earnedEnergy) / energyInCharge);

            if (num < charges.Count) {
                StartCoroutine(Show(num));
            }
        }

        IEnumerator Show(int chargeIndex) {
            yield return new WaitForSeconds(duration);

            for (int i = chargeIndex; i < charges.Count; i++) {
                float chargeProgress = Mathf.Clamp01((currentEnergy - i * energyInCharge) / (float)energyInCharge);
                charges[i].Set(previousProgress[i], chargeProgress);
                yield return new WaitForSeconds(duration);
            }
        }
    }
}
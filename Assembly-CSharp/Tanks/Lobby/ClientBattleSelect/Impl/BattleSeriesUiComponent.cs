using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSeriesUiComponent : BehaviourComponent {
        [Header("Colors")] [SerializeField] Color _battleSeriesColor;

        [SerializeField] Color _deserterColor;

        [SerializeField] Color _defaultColor;

        [Header("Texts")] [SerializeField] LocalizedField _battlesAmountSingularText;

        [SerializeField] LocalizedField _battlesAmountPlural1Text;

        [SerializeField] LocalizedField _battlesAmountPlural2Text;

        [SerializeField] TextMeshProUGUI _battleSeriesText;

        [SerializeField] LocalizedField _battleSeriesString;

        [SerializeField] LocalizedField _deserterString;

        [Space(10f)] [SerializeField] TextMeshProUGUI _additionalExpText;

        [SerializeField] LocalizedField _additionalExpString;

        [SerializeField] TextMeshProUGUI _additionScoresText;

        [SerializeField] LocalizedField _additionScoresString;

        [SerializeField] TextMeshProUGUI _additionalMessageText;

        [SerializeField] LocalizedField _nextBattleNotificationString;

        [SerializeField] LocalizedField _maxSeriesAchiviedString;

        [SerializeField] LocalizedField _deserterAdditionalMessageString;

        [Header("Main Icon")] [SerializeField] TextMeshProUGUI _battleSeriesCountText;

        [SerializeField] ImageSkin _battleSeriesImage;

        [SerializeField] string[] _battleImageUids;

        [SerializeField] string _deserterImageUid;

        public int CurrentBattleCount {
            set {
                if (value > 0) {
                    _battleSeriesText.color = _battleSeriesColor;
                    _battleSeriesText.text = string.Format(_battleSeriesString, value);
                    _battleSeriesCountText.color = _battleSeriesColor;
                    string text = ArabianToRomanNumConverter.ArabianToRoman(value);
                    _battleSeriesCountText.text = text;
                } else {
                    _battleSeriesText.color = _defaultColor;
                    _battleSeriesText.text = string.Format(_deserterString, Pluralize(Mathf.Abs(value)));
                }
            }
        }

        public float BattleSeriesPercent {
            set {
                if (value > 0f) {
                    bool flag = value >= 1f;
                    _battleSeriesCountText.gameObject.SetActive(!flag);
                    _additionalMessageText.text = !flag ? _nextBattleNotificationString : _maxSeriesAchiviedString;
                    _battleSeriesImage.SpriteUid = !flag ? _battleImageUids[(int)Mathf.Floor((_battleImageUids.Length - 1) * value)] : _battleImageUids[_battleImageUids.Length - 1];
                    _battleSeriesImage.GetComponent<Image>().color = _defaultColor;
                } else {
                    _additionalMessageText.text = _deserterAdditionalMessageString;
                    _battleSeriesCountText.gameObject.SetActive(false);
                    _battleSeriesImage.SpriteUid = _deserterImageUid;
                    _battleSeriesImage.GetComponent<Image>().color = _deserterColor;
                }
            }
        }

        public float ExperienceMultiplier {
            set {
                if (value > 1f) {
                    _additionalExpText.gameObject.SetActive(true);
                    string text = string.Format("{0:0}", value * 100f - 100f);
                    _additionalExpText.text = string.Format(_additionalExpString, "<color=#" + _battleSeriesColor.ToHexString() + ">+" + text + "%</color>");
                } else {
                    _additionalExpText.gameObject.SetActive(false);
                }
            }
        }

        public float ContainerScoreMultiplier {
            set {
                if (value > 1f) {
                    _additionScoresText.gameObject.SetActive(true);
                    string text = string.Format("{0:0}", value * 100f - 100f);
                    _additionScoresText.text = string.Format(_additionScoresString, "<color=#" + _battleSeriesColor.ToHexString() + ">+" + text + "%</color>");
                } else {
                    _additionScoresText.gameObject.SetActive(false);
                }
            }
        }

        string Pluralize(int amount) {
            switch (CasesUtil.GetCase(amount)) {
                case CaseType.DEFAULT:
                    return string.Format(_battlesAmountPlural1Text.Value, amount);

                case CaseType.ONE:
                    return string.Format(_battlesAmountSingularText.Value, amount);

                case CaseType.TWO:
                    return string.Format(_battlesAmountPlural2Text.Value, amount);

                default:
                    throw new Exception("ivnalid case");
            }
        }
    }
}
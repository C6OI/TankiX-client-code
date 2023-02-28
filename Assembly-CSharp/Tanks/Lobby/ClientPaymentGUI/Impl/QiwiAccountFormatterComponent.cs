using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class QiwiAccountFormatterComponent : BehaviourComponent {
        [SerializeField] List<int> spaces;

        ICollection<string> codes;

        readonly Regex commonPhoneRegex = new("^[1-9]{1}[0-9]{3,18}$");

        readonly Regex digitsOnly = new("[^\\d]");

        readonly StringBuilder formated = new(30);

        bool formating;

        protected TMP_InputField input;

        public bool IsValidPhoneNumber {
            get {
                string text = ClearFormat(input.text);

                if (codes == null) {
                    return false;
                }

                foreach (string code in codes) {
                    if (text.StartsWith(code)) {
                        return commonPhoneRegex.IsMatch(text);
                    }
                }

                return false;
            }
        }

        public string Account => "+" + ClearFormat(input.text);

        void Awake() {
            input = GetComponent<TMP_InputField>();
            input.onValueChanged.AddListener(Format);
        }

        void OnEnable() {
            StartCoroutine(DelayFocus());
        }

        public void SetCodes(ICollection<string> codes) {
            this.codes = codes;
        }

        IEnumerator DelayFocus() {
            yield return new WaitForSeconds(0.1f);

            TMP_InputField tMP_InputField = input;
            int length = input.text.Length;
            input.selectionFocusPosition = length;
            tMP_InputField.selectionAnchorPosition = length;
        }

        void Format(string text) {
            if (formating) {
                return;
            }

            GetComponent<Animator>().SetBool("HasError", false);
            formating = true;
            formated.Length = 0;
            int num = input.stringPosition - GetSpacesBeforeCarret();
            text = ClearFormat(text);
            string text2 = string.Empty;

            foreach (string code in codes) {
                if (text.StartsWith(code) && text2.Length < code.Length) {
                    text2 = code;
                    break;
                }
            }

            int num2 = 0;
            num2 += text2.Length;
            formated.Append(text2);
            int num3 = 0;
            int num4 = 0;
            int num5;

            for (; num2 < text.Length; num2 += num5) {
                if (num3 < spaces.Count) {
                    num5 = Math.Min(text.Length - num2, spaces[num3++]);

                    if (num2 <= num) {
                        num4++;
                    }

                    formated.Append(" ");
                    formated.Append(text.Substring(num2, num5));
                    continue;
                }

                formated.Append(text.Substring(num2));
                break;
            }

            input.text = formated.ToString();
            input.stringPosition = num + num4;
            input.caretPosition = num + num4;
            formating = false;
        }

        int GetSpacesBeforeCarret() {
            int num = 0;

            for (int i = 0; i < input.stringPosition; i++) {
                if (input.text[i] == ' ') {
                    num++;
                }
            }

            return num;
        }

        string ClearFormat(string text) => digitsOnly.Replace(text, string.Empty);
    }
}
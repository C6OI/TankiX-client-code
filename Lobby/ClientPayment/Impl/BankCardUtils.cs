using System.Collections.Generic;

namespace Lobby.ClientPayment.Impl {
    public class BankCardUtils {
        static readonly Dictionary<BankCardType, HashSet<string>> validStarts = GenerateStarts();

        static readonly Dictionary<BankCardType, HashSet<int>> validLengths = GenerateLengths();

        static Dictionary<BankCardType, HashSet<string>> GenerateStarts() {
            Dictionary<BankCardType, HashSet<string>> dictionary = new();
            dictionary.Add(BankCardType.AMERICAN_EXPRESS, new HashSet<string> { "34", "37" });
            dictionary.Add(BankCardType.CHINA_UNIONPAY, new HashSet<string> { "62" });

            dictionary.Add(BankCardType.DINERS_CLUB_CARTE_BLANCHE,
                new HashSet<string> { "300", "301", "302", "303", "304", "305" });

            dictionary.Add(BankCardType.DINERS_CLUB_INTERNATIONAL,
                new HashSet<string> { "300", "301", "302", "303", "304", "305", "309", "36" });

            dictionary.Add(BankCardType.DINERS_CLUB_UNITED_STATES_AND_CANADA, new HashSet<string> { "54", "55" });

            dictionary.Add(BankCardType.DISCOVERY_CARD,
                new HashSet<string> { "6011", "644", "645", "646", "647", "648", "649", "622" });

            dictionary.Add(BankCardType.INTERPAYMENT, new HashSet<string> { "636" });
            dictionary.Add(BankCardType.INSTAPAYMENT, new HashSet<string> { "637", "638", "639" });
            HashSet<string> hashSet = new();

            for (int i = 3528; i <= 3589; i++) {
                hashSet.Add(i.ToString());
            }

            dictionary.Add(BankCardType.JCB, hashSet);
            hashSet = new HashSet<string>();

            for (int j = 56; j <= 69; j++) {
                hashSet.Add(j.ToString());
            }

            hashSet.Add("50");
            dictionary.Add(BankCardType.MAESTRO, hashSet);
            dictionary.Add(BankCardType.DANKORT, new HashSet<string> { "4175", "4571", "639" });
            dictionary.Add(BankCardType.NSPK_MIR, new HashSet<string> { "2200", "2201", "2202", "2203", "2204" });
            hashSet = new HashSet<string>();

            for (int k = 2221; k <= 2720; k++) {
                hashSet.Add(k.ToString());
            }

            for (int l = 51; l <= 55; l++) {
                hashSet.Add(l.ToString());
            }

            dictionary.Add(BankCardType.MASTERCARD, hashSet);
            dictionary.Add(BankCardType.VISA, new HashSet<string> { "4" });
            dictionary.Add(BankCardType.UATP, new HashSet<string> { "1" });
            dictionary.Add(BankCardType.CARDGUARD_EAD_BG_ILS, new HashSet<string> { "5392" });
            return dictionary;
        }

        static Dictionary<BankCardType, HashSet<int>> GenerateLengths() {
            Dictionary<BankCardType, HashSet<int>> dictionary = new();
            dictionary.Add(BankCardType.AMERICAN_EXPRESS, new HashSet<int> { 15 });
            dictionary.Add(BankCardType.CHINA_UNIONPAY, new HashSet<int> { 16, 17, 18, 19 });
            dictionary.Add(BankCardType.DINERS_CLUB_CARTE_BLANCHE, new HashSet<int> { 14 });
            dictionary.Add(BankCardType.DINERS_CLUB_INTERNATIONAL, new HashSet<int> { 14 });
            dictionary.Add(BankCardType.DINERS_CLUB_UNITED_STATES_AND_CANADA, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.DISCOVERY_CARD, new HashSet<int> { 16, 19 });
            dictionary.Add(BankCardType.INTERPAYMENT, new HashSet<int> { 16, 17, 18, 19 });
            dictionary.Add(BankCardType.INSTAPAYMENT, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.JCB, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.MAESTRO, new HashSet<int> { 12, 13, 14, 15, 16, 17, 18, 19 });
            dictionary.Add(BankCardType.DANKORT, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.NSPK_MIR, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.MASTERCARD, new HashSet<int> { 16 });
            dictionary.Add(BankCardType.VISA, new HashSet<int> { 13, 16, 19 });
            dictionary.Add(BankCardType.UATP, new HashSet<int> { 15 });
            dictionary.Add(BankCardType.CARDGUARD_EAD_BG_ILS, new HashSet<int> { 16 });
            return dictionary;
        }

        public static BankCardType GetBankCardType(string number) {
            string text = number.Replace(" ", string.Empty);

            foreach (KeyValuePair<BankCardType, HashSet<string>> validStart in validStarts) {
                foreach (string item in validStart.Value) {
                    if (text.StartsWith(item) && validLengths[validStart.Key].Contains(text.Length)) {
                        return validStart.Key;
                    }
                }
            }

            return BankCardType.INVALID;
        }

        public static bool IsBankCard(string number) => GetBankCardType(number) != BankCardType.INVALID;
    }
}
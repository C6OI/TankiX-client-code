namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public static class ArabianToRomanNumConverter {
        static readonly int[] arabian = new int[13] {
            1, 4, 5, 9, 10, 40, 50, 90, 100, 400,
            500, 900, 1000
        };

        static readonly string[] roman = new string[13] {
            "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD",
            "D", "CM", "M"
        };

        public static string ArabianToRoman(int arabianNum) {
            string text = string.Empty;
            int num = arabian.Length - 1;

            while (arabianNum > 0) {
                if (arabianNum >= arabian[num]) {
                    text += roman[num];
                    arabianNum -= arabian[num];
                } else {
                    num--;
                }
            }

            return text;
        }

        public static int RomanToArabian(string romanNum) {
            romanNum = romanNum.ToUpper();
            int num = 0;
            int num2 = arabian.Length - 1;
            int num3 = 0;

            while (num2 >= 0 && num3 < romanNum.Length) {
                if (romanNum.Substring(num3, roman[num2].Length) == roman[num2]) {
                    num += arabian[num2];
                    num3 += roman[num2].Length;
                } else {
                    num2--;
                }
            }

            return num;
        }
    }
}
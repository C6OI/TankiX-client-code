using System.Text.RegularExpressions;
using Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class CapsInputFormatter : BaseInputFormatter {
        static readonly Regex allowedSymbols = new("[A-Za-z ]");

        protected override string FormatAt(char symbol, int charIndex) {
            string text = symbol.ToString();

            if (!allowedSymbols.IsMatch(text)) {
                return string.Empty;
            }

            return text.ToUpper();
        }

        protected override string ClearFormat(string text) => text;
    }
}
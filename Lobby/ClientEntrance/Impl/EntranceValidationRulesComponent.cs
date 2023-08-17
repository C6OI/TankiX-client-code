using System.Text.RegularExpressions;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.Impl {
    public class EntranceValidationRulesComponent : Component {
        static readonly Regex MATCH_EVERYTHING = new(".+");

        static readonly Regex MATCH_NOTHING = new("(?!)");

        Regex emailRegex = MATCH_NOTHING;

        Regex loginBeginingRegex = MATCH_EVERYTHING;

        Regex loginEndingRegex = MATCH_EVERYTHING;

        Regex loginRegex = MATCH_EVERYTHING;

        Regex loginSpecTogetherRegex = MATCH_NOTHING;

        Regex loginSymbolsRegex = MATCH_EVERYTHING;

        Regex passwordRegex = MATCH_EVERYTHING;

        public int minLoginLength { get; set; }

        public int maxLoginLength { get; set; }

        public int minPasswordLength { get; set; }

        public int maxPasswordLength { get; set; }

        public int maxCaptchaLength { get; set; }

        public int minEmailLength { get; set; }

        public int maxEmailLength { get; set; }

        public string LoginRegex {
            get => loginRegex.ToString();
            set => loginRegex = new Regex(value);
        }

        public string LoginSymbolsRegex {
            get => loginSymbolsRegex.ToString();
            set => loginSymbolsRegex = new Regex(value);
        }

        public string LoginBeginingRegex {
            get => loginBeginingRegex.ToString();
            set => loginBeginingRegex = new Regex(value);
        }

        public string LoginEndingRegex {
            get => loginEndingRegex.ToString();
            set => loginEndingRegex = new Regex(value);
        }

        public string LoginSpecTogetherRegex {
            get => loginSpecTogetherRegex.ToString();
            set => loginSpecTogetherRegex = new Regex(value);
        }

        public string PasswordRegex {
            get => passwordRegex.ToString();
            set => passwordRegex = new Regex(value);
        }

        public string EmailRegex {
            get => emailRegex.ToString();
            set => emailRegex = new Regex(value);
        }

        public bool IsEmailValid(string email) => emailRegex.IsMatch(email);

        public bool IsLoginSymbolsValid(string login) => loginSymbolsRegex.IsMatch(login);

        public bool IsLoginBeginingValid(string login) => loginBeginingRegex.IsMatch(login);

        public bool IsLoginEndingValid(string login) => loginEndingRegex.IsMatch(login);

        public bool AreSpecSymbolsTogether(string login) => loginSpecTogetherRegex.IsMatch(login);

        public bool IsPasswordSymbolsValid(string password) => passwordRegex.IsMatch(password);

        public bool IsLoginTooShort(string login) => login.Length < minLoginLength;

        public bool IsLoginTooLong(string login) => login.Length > maxLoginLength;

        public bool IsPasswordTooShort(string password) => password.Length < minPasswordLength;

        public bool IsPasswordTooLong(string password) => password.Length > maxPasswordLength;

        public bool IsLoginValid(string login) => !IsLoginTooShort(login) &&
                                                  !IsLoginTooLong(login) &&
                                                  IsLoginSymbolsValid(login) &&
                                                  IsLoginBeginingValid(login) &&
                                                  IsLoginEndingValid(login) &&
                                                  !AreSpecSymbolsTogether(login) &&
                                                  loginRegex.IsMatch(login);
    }
}
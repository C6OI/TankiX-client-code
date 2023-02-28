namespace Tanks.Lobby.ClientNavigation.API {
    public class ErrorScreenData {
        public static ErrorScreenData data;

        public string HeaderText { get; set; }

        public string ErrorText { get; set; }

        public string RestartButtonLabel { get; set; }

        public string ExitButtonLabel { get; set; }

        public string ReportButtonLabel { get; set; }

        public string ReportUrl { get; set; }

        public int ReConnectTime { get; set; }
    }
}
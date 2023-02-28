using System;
using System.Linq;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public class FatalErrorHandler {
        static bool alreadyHandling;

        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        public static bool IsErrorScreenWasShown { get; set; }

        public static void HandleLog(string logString, string stackTrace, LogType type) {
            if (IsErrorScreenWasShown || alreadyHandling) {
                return;
            }

            bool flag = type == LogType.Exception && !SkipShowScreen(logString, stackTrace);

            if ((type == LogType.Error || type == LogType.Exception) && !LogFromLog4Net(logString) && !SkipSendToLog(logString)) {
                LoggerProvider.GetLogger<FatalErrorHandler>().ErrorFormat("{0}\n\n{1}", logString, stackTrace);
            }

            flag &= !Application.isEditor;

            if (!(flag & !Environment.GetCommandLineArgs().Contains("-ignoreerrors"))) {
                return;
            }

            alreadyHandling = true;

            try {
                ShowFatalErrorScreen();
            } finally {
                alreadyHandling = false;
            }
        }

        public static void ShowBrokenConfigsErrorScreen() {
            if (!IsErrorScreenWasShown) {
                IsErrorScreenWasShown = true;
                ErrorScreenData errorScreenData = new();
                errorScreenData.HeaderText = "ERROR";
                errorScreenData.ErrorText = "Required resources are corrupted or missing";
                errorScreenData.ReportButtonLabel = "REPORT";
                errorScreenData.ReportUrl = "https://help.tankix.com/en/tanki-x/articles/issues/initialization-issue";
                errorScreenData.ReConnectTime = 999999;
                errorScreenData.ExitButtonLabel = "EXIT";
                ErrorScreenData.data = errorScreenData;
                SceneSwitcher.CleanAndSwitch(SceneNames.FATAL_ERROR);
            }
        }

        public static void ShowFatalErrorScreen(string configPath = "clientlocal/ui/screen/error/unexpected") {
            if (IsErrorScreenWasShown) {
                return;
            }

            IsErrorScreenWasShown = true;

            if (ConfiguratorService.HasConfig("clientlocal/ui/screen/error/common")) {
                ErrorScreenData errorScreenData = LoadStringsFromConfig("clientlocal/ui/screen/error/common");

                if (ConfiguratorService.HasConfig(configPath)) {
                    ErrorScreenData configFrom = LoadStringsFromConfig(configPath);
                    OverwriteNonEmptyFields(configFrom, errorScreenData);
                }

                ErrorScreenData.data = errorScreenData;
            }

            SceneSwitcher.CleanAndSwitch(SceneNames.FATAL_ERROR);
        }

        static void OverwriteNonEmptyFields(ErrorScreenData configFrom, ErrorScreenData configTo) {
            if (!string.IsNullOrEmpty(configFrom.HeaderText)) {
                configTo.HeaderText = configFrom.HeaderText;
            }

            if (!string.IsNullOrEmpty(configFrom.ErrorText)) {
                configTo.ErrorText = configFrom.ErrorText;
            }

            if (!string.IsNullOrEmpty(configFrom.RestartButtonLabel)) {
                configTo.RestartButtonLabel = configFrom.RestartButtonLabel;
            }

            if (!string.IsNullOrEmpty(configFrom.ExitButtonLabel)) {
                configTo.ExitButtonLabel = configFrom.ExitButtonLabel;
            }

            if (!string.IsNullOrEmpty(configFrom.ReportButtonLabel)) {
                configTo.ReportButtonLabel = configFrom.ReportButtonLabel;
            }

            if (!string.IsNullOrEmpty(configFrom.ReportUrl)) {
                configTo.ReportUrl = configFrom.ReportUrl;
            }

            configTo.ReConnectTime = configFrom.ReConnectTime;
        }

        static ErrorScreenData LoadStringsFromConfig(string configPath) {
            YamlNode config = ConfiguratorService.GetConfig(configPath);
            return config.ConvertTo<ErrorScreenData>();
        }

        static bool LogFromLog4Net(string logString) {
            if (logString != null && logString.StartsWith("log4net:")) {
                return true;
            }

            return false;
        }

        static bool SkipShowScreen(string logString, string stackTrace) {
            if (logString != null && logString.StartsWith("IndexOutOfRangeException") && stackTrace.Contains("TMPro.TMP_Text.FillCharacterVertexBuffers (Int32 i, Int32 index_X4)")) {
                return true;
            }

            return false;
        }

        static bool SkipSendToLog(string logString) {
            if (logString != null && (logString.Contains("The AssetBundle") || logString.StartsWith("Failed opening GI file") ||
                                      logString.StartsWith("Failed loading Enlighten probe set data for hash") || logString.StartsWith("Error adding Enlighten probeset"))) {
                return true;
            }

            return false;
        }
    }
}
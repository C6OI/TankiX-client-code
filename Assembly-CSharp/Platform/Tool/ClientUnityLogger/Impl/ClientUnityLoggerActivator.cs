using System;
using System.IO;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.Impl {
    public class ClientUnityLoggerActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() {
            if (!Application.isEditor) {
                InitLogger();
            }
        }

        public static void InitLogger() {
            string text = ConfigPath.ConvertToUrl(Application.streamingAssetsPath + "/" + ConfigPath.CLIENT_LOGGER_CONFIG);
            Console.WriteLine("Load client logger config: " + text);
            WWW wWW = new(text);

            while (!wWW.isDone) { }

            if (!string.IsNullOrEmpty(wWW.error)) {
                Debug.LogError("Error loading logger config from: " + text + " Error: " + wWW.error);
            } else {
                LoggerProvider.LoadConfiguration(new MemoryStream(wWW.bytes));
            }
        }
    }
}
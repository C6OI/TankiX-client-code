using System.IO;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.Impl {
    public class ClientUnityLoggerActivator : DefaultActivator<AutoCompleting> {
        protected override void Activate() {
            string text = ConfigPath.ConvertToUrl(Application.dataPath + "/log4net.xml");
            Debug.Log("load client logger config: " + text);
            WWW wWW = new(text);

            while (!wWW.isDone) { }

            if (string.IsNullOrEmpty(wWW.error)) {
                LoggerProvider.LoadConfiguration(new MemoryStream(wWW.bytes));
            } else {
                Debug.LogError("Error load client logger config form: " + text + " Error: " + wWW.error);
            }
        }
    }
}
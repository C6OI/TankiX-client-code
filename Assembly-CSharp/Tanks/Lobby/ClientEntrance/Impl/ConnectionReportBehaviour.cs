using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class ConnectionReportBehaviour : MonoBehaviour {
        public float connectionWaitTime = 30f;

        float startTime;
        public ConnectionReportSystem system;

        void Update() {
            if (Time.realtimeSinceStartup > startTime + connectionWaitTime) {
                if (!system.hasConnection) {
                    string text = "Client did not receive ClientSession in " + connectionWaitTime + " seconds. ";

                    if (InitConfiguration.Config != null) {
                        string text2 = text;
                        text = text2 + " InitConfig: " + InitConfiguration.Config.Host + ":" + InitConfiguration.Config.AcceptorPort;
                    }

                    LoggerProvider.GetLogger<ConnectionReportActivator>().Error(text, new ClientSessionLoadTimeoutException());
                }

                Destroy(gameObject);
            }

            if (system.hasConnection) {
                LoggerProvider.GetLogger<ConnectionReportActivator>().Info("ClientStarted elapsed=" + (Time.realtimeSinceStartup - startTime));
                Destroy(gameObject);
            }
        }

        void OnEnable() {
            LoggerProvider.GetLogger<ConnectionReportActivator>().Info("StartClient");
            startTime = Time.realtimeSinceStartup;
        }
    }
}
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class ServerConnectionBehaviour : MonoBehaviour {
        string hostName = "localhost";

        int[] ports = new int[1] { 5090 };

        Rect windowRect;

        [Inject] public static NetworkService NetworkService { get; set; }

        public void LateUpdate() {
            if (NetworkService.Connected) {
                NetworkService.ProcessNetworkTasks();
                return;
            }

            enabled = false;

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("ServerDisconnected");
                engine.ScheduleEvent<ServerDisconnectedEvent>(entity);
            });
        }

        public void OnApplicationQuit() => DisconnectIfConnected();

        public void OpenConnection(string hostName, int[] ports) {
            this.hostName = hostName;
            this.ports = ports;
            DisconnectIfConnected();
            NetworkService.Connect(hostName, ports);
        }

        void DisconnectIfConnected() {
            if (NetworkService.Connected) {
                NetworkService.Disconnect();
            }
        }
    }
}
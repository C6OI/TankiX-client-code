using System;
using System.Linq;
using System.Net.Sockets;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class ServerConnectionBehaviour : MonoBehaviour {
        const int DISCONNECTED = 0;

        const int CONNECTED = 1;

        const int CONNECTION_ERROR = 2;

        volatile int connectionStatus;

        string host;

        bool isConnecting;

        int networkSliceTime;

        int portIndex;

        int[] ports = new int[1] { 5090 };

        [Inject] public static NetworkService NetworkService { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public int NetworkMaxDelayTime { get; set; }

        public void Update() {
            if (isConnecting) {
                CheckConnectionStatus();
                return;
            }

            if (NetworkService.IsConnected) {
                NetworkService.ReadAndExecuteCommands(networkSliceTime, NetworkMaxDelayTime);
                return;
            }

            enabled = false;
            Engine engine = EngineService.Engine;
            LogConnectionError("Server disconnected: ", host, ports);
            Entity entity = engine.CreateEntity("ServerDisconnected");
            engine.ScheduleEvent<ServerDisconnectedEvent>(entity);
        }

        public void LateUpdate() {
            if (NetworkService.IsConnected) {
                Flow.Current.Finish();
                NetworkService.WriteCommands();
                Flow.Current.Clean();
            }
        }

        public void OnDestroy() {
            DisconnectIfConnected();
        }

        public void OnApplicationQuit() {
            DisconnectIfConnected();
        }

        public event Action CompleteEvent;

        public void OpenConnection(string host, int[] ports) {
            OpenConnection(host, ports, 0, 0, null, false);
        }

        public void OpenConnection(string host, int[] ports, int networkSliceTime, int networkMaxDelayTime, Action completeAction, bool splitShareCommand) {
            if (NetworkService.IsConnected) {
                Debug.LogWarning("Already connected.");
                return;
            }

            NetworkService.SplitShareCommand = splitShareCommand;
            this.host = host;
            this.networkSliceTime = networkSliceTime;
            NetworkMaxDelayTime = networkMaxDelayTime;

            if (ports.Length > 0) {
                this.ports = ports;
            }

            PrefetchSocketPolicyForWebplayer(host);
            TryConnect(completeAction);
        }

        void PrefetchSocketPolicyForWebplayer(string host) { }

        void TryConnect(Action completeAction = null) {
            CompleteEvent += delegate {
                connectionStatus = NetworkService.IsConnected ? 1 : 2;
            };

            if (completeAction != null) {
                CompleteEvent += completeAction;
            }

            isConnecting = true;
            NetworkService.ConnectAsync(host, ports[portIndex], CompleteEvent);
        }

        void CheckConnectionStatus() {
            if (connectionStatus == 1) {
                isConnecting = false;
            } else {
                if (connectionStatus != 2) {
                    return;
                }

                connectionStatus = 0;
                portIndex++;

                if (portIndex < ports.Length) {
                    try {
                        TryConnect();
                        return;
                    } catch (SocketException) {
                        SendNoServerConnection();
                        return;
                    }
                }

                enabled = false;
                SendNoServerConnection();
            }
        }

        void SendNoServerConnection() {
            LogConnectionError("Could not connect: ", host, ports);
            Engine engine = EngineService.Engine;
            Entity entity = engine.CreateEntity("NoConnection");
            engine.ScheduleEvent<NoServerConnectionEvent>(entity);
        }

        void DisconnectIfConnected() {
            if (NetworkService != null && NetworkService.IsConnected) {
                NetworkService.Disconnect();
            }
        }

        public bool IsConnecting() => isConnecting;

        static void LogConnectionError(string message, string host, int[] ports) {
            LoggerProvider.GetLogger<ServerConnectionBehaviour>().ErrorFormat("{0}{1}:{2}", message, host, string.Join(",", ports.Select(p => p.ToString()).ToArray()));
        }
    }
}
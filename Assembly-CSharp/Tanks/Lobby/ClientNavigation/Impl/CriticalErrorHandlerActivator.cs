using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class CriticalErrorHandlerActivator : UnityAwareActivator<AutoCompleting> {
        protected override void Activate() {
            Application.logMessageReceivedThreaded += FatalErrorHandler.HandleLog;
        }
    }
}
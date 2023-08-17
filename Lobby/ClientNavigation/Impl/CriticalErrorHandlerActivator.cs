using Lobby.ClientNavigation.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using UnityEngine;

namespace Lobby.ClientNavigation.Impl {
    public class CriticalErrorHandlerActivator : UnityAwareActivator<AutoCompleting> {
        protected override void Activate() => Application.logMessageReceivedThreaded += FatalErrorHandler.HandleLog;
    }
}
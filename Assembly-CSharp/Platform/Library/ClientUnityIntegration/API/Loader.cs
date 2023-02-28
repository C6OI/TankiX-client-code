using System;

namespace Platform.Library.ClientUnityIntegration.API {
    public interface Loader : IDisposable {
        byte[] Bytes { get; }

        float Progress { get; }

        bool IsDone { get; }

        string URL { get; }

        string Error { get; }
    }
}
using System;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public interface Loader : IDisposable {
        AssetBundle AssetBundle { get; }

        byte[] Bytes { get; }

        float Progress { get; }

        bool IsDone { get; }

        string URL { get; }

        string Error { get; }
    }
}
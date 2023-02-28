using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UnusedAssetCleaner : MonoBehaviour {
        AsyncOperation _cleanOperation;

        void OnEnable() {
            _cleanOperation = Resources.UnloadUnusedAssets();
            StartCoroutine(CleanCoroutine());
            GC.Collect();
        }

        IEnumerator CleanCoroutine() {
            if (!_cleanOperation.isDone) {
                Console.WriteLine("Start cleaning in " + DateTime.Now);
                Console.WriteLine("UsedMemory: " + Process.GetCurrentProcess().WorkingSet64);
            }

            yield return _cleanOperation;

            if (_cleanOperation.isDone) {
                Console.WriteLine("Stop cleaning in " + DateTime.Now);
                Console.WriteLine("UsedMemory: " + Process.GetCurrentProcess().WorkingSet64);
            }
        }
    }
}
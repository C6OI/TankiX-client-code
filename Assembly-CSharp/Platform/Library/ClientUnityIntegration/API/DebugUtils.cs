using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Platform.Library.ClientUnityIntegration.API {
    public class DebugUtils {
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) {
            Debug.DrawLine(start, end, color, duration);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color) {
            Debug.DrawLine(start, end, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end) {
            Debug.DrawLine(start, end);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration, bool depthTest) {
            Debug.DrawRay(start, direction, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration) {
            Debug.DrawRay(start, direction, color, duration);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 direction, Color color) {
            Debug.DrawRay(start, direction, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 direction) {
            Debug.DrawRay(start, direction);
        }
    }
}
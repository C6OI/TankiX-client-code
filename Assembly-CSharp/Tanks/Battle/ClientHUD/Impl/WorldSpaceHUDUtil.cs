using System;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WorldSpaceHUDUtil {
        public static void ScaleToRealSize(Transform canvasTransform, Transform elementTransform, Camera camera) {
            float num = camera.pixelHeight / (2f * Mathf.Tan((float)Math.PI / 360f * camera.fieldOfView));
            Vector3 vector = camera.WorldToViewportPoint(elementTransform.position);
            float num2 = 1f / canvasTransform.localScale.x;
            float num3 = vector.z / num * num2;
            elementTransform.localScale = new Vector3(num3, num3, num3);
        }
    }
}
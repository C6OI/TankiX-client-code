using System.IO;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class ScreenShotUtil {
        public static void TakeScreenshot(Camera camera, string filePath, int scale = 1) {
            int num = camera.pixelWidth * scale;
            int num2 = camera.pixelHeight * scale;
            RenderTexture renderTexture2 = camera.targetTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
            camera.Render();
            RenderTexture.active = renderTexture2;
            Texture2D texture2D = new(num, num2, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture2);
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filePath));
        }

        public static byte[] TakeScreenshotBytes(Camera camera, int scale = 1) {
            int num = camera.pixelWidth * scale;
            int num2 = camera.pixelHeight * scale;
            RenderTexture renderTexture2 = camera.targetTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
            camera.Render();
            RenderTexture.active = renderTexture2;
            Texture2D texture2D = new(num, num2, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture2);
            return texture2D.EncodeToPNG();
        }

        public static RenderTexture TakeScreenshotTexture(Camera camera, float scale = 1f) {
            int width = (int)(camera.pixelWidth * scale);
            int height = (int)(camera.pixelHeight * scale);
            RenderTexture result = camera.targetTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            camera.Render();
            camera.targetTexture = null;
            RenderTexture.active = null;
            return result;
        }

        public static void TakeScreenshotAndOpenIt(Camera camera, string filePath, int scale = 1) {
            TakeScreenshot(camera, filePath, scale);
            Application.OpenURL(filePath);
        }
    }
}
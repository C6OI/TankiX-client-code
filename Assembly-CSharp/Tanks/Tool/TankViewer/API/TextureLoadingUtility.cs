using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class TextureLoadingUtility {
        public static Texture2D CreateTextureWithGamma(Texture2D texture) {
            Texture2D texture2D = new(texture.width, texture.height, texture.format, texture.mipmapCount > 1, false);
            texture2D.LoadRawTextureData(texture.GetRawTextureData());
            texture2D.Apply();
            return texture2D;
        }

        public static Texture2D CreateNormalMap(Texture2D source) {
            Texture2D texture2D = new(source.width, source.height, TextureFormat.ARGB32, true, true);
            Color color = default;

            for (int i = 0; i < source.width; i++) {
                for (int j = 0; j < source.height; j++) {
                    color.b = color.g = color.r = source.GetPixel(i, j).g;
                    color.a = source.GetPixel(i, j).r;
                    texture2D.SetPixel(i, j, color);
                }
            }

            texture2D.Apply();
            return texture2D;
        }
    }
}
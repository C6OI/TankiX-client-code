using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class Mask {
        readonly Texture2D texture;

        public Mask(Texture2D texture, float blackThreshold, bool whiteAsEmpty) {
            this.texture = texture;
            MarkedPixels = new List<Vector2>();

            for (int i = 0; i < texture.width; i++) {
                for (int j = 0; j < texture.height; j++) {
                    if (IsMaskPixel(i, j, blackThreshold, whiteAsEmpty)) {
                        MarkedPixels.Add(new Vector2(i, j));
                    }
                }
            }
        }

        public int Width => texture.width;

        public int Height => texture.height;

        public List<Vector2> MarkedPixels { get; }

        bool IsMaskPixel(int i, int j, float blackThreshold, bool whiteAsEmpty) {
            Color pixel = texture.GetPixel(i, j);
            return !whiteAsEmpty ? pixel.r > blackThreshold : pixel.r <= blackThreshold;
        }
    }
}
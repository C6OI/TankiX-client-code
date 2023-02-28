using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AnimatedSprite3D : Sprite3D {
        public int tilesX = 1;

        public int tilesY = 1;

        public int fps = 24;

        public int framesCount;

        public bool loop = true;

        float initTime;

        int lastIndex = -1;

        Vector2 tileSize;

        [Inject] public static UnityTime UnityTime { get; set; }

        protected new void Start() {
            base.Start();
            useInstanceMaterial = true;
            UpdateMaterial();
            initTime = UnityTime.time;
        }

        public void ResetMaterial(Material newMaterial) {
            assetMaterial = newMaterial;
            material = new Material(newMaterial);
            lastIndex = -1;
        }

        public override void Draw() {
            useInstanceMaterial = true;
            AnimateMaterialTile();
            base.Draw();
        }

        void AnimateMaterialTile() {
            tileSize = new Vector2(1f / tilesX, 1f / tilesY);
            int num = framesCount <= 0 ? tilesX * tilesY : framesCount;
            int num2 = (int)((UnityTime.time - initTime) * fps);

            if (loop) {
                num2 %= num;
            } else if (num2 >= num) {
                num2 = num - 1;
            }

            if (num2 != lastIndex) {
                int num3 = num2 % tilesX;
                int num4 = num2 / tilesY;
                Vector2 value = new(num3 * tileSize.x, 1f - tileSize.y - num4 * tileSize.y);
                material.SetTextureOffset("_MainTex", value);
                material.SetTextureScale("_MainTex", tileSize);
                lastIndex = num2;
            }
        }
    }
}
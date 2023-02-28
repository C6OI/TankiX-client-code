using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeBarImage : MaskableGraphic {
        [SerializeField] float spacing = 2f;

        [SerializeField] int segmentsCount = 30;

        [SerializeField] Sprite sprite;

        [SerializeField] PaletteColorField filledColor;

        [SerializeField] PaletteColorField upgradeColor;

        [SerializeField] PaletteColorField backgroundColor;

        [SerializeField] PaletteColorField strokeColor;

        public int SegmentsCount {
            get => segmentsCount;
            set => segmentsCount = value;
        }

        public int CurrentSegment { get; set; }

        public int UpgradedSegments { get; set; }

        public float CurrentSegmentProgress { get; set; } = 1f;

        public float CurrentSegmentScale { get; set; } = 30f;

        public override Texture mainTexture => !(sprite != null) ? base.mainTexture : sprite.texture;

        public float pixelsPerUnit {
            get {
                float num = 100f;

                if ((bool)sprite) {
                    num = sprite.pixelsPerUnit;
                }

                float num2 = 100f;

                if ((bool)canvas) {
                    num2 = canvas.referencePixelsPerUnit;
                }

                return num / num2;
            }
        }

        public void Validate() {
            UpdateGeometry();
        }

        protected override void UpdateMaterial() {
            base.UpdateMaterial();

            if (sprite == null) {
                canvasRenderer.SetAlphaTexture(null);
                return;
            }

            Texture2D associatedAlphaSplitTexture = sprite.associatedAlphaSplitTexture;

            if (associatedAlphaSplitTexture != null) {
                canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            if (segmentsCount > 0 && sprite != null) {
                Border(vh);
            }
        }

        void Border(VertexHelper vh) {
            Rect pixelAdjustedRect = GetPixelAdjustedRect();
            float num = pixelAdjustedRect.xMin;
            float num2 = spacing / pixelsPerUnit;
            float num3 = (pixelAdjustedRect.width - num2 * (segmentsCount - 1)) / (segmentsCount - 1 + CurrentSegmentScale);
            Vector4 outerUV = DataUtility.GetOuterUV(sprite);
            Vector4 innerUV = DataUtility.GetInnerUV(sprite);
            Vector2 size = sprite.rect.size;
            Vector4 adjustedBorders = GetAdjustedBorders(sprite.border / pixelsPerUnit, pixelAdjustedRect);
            float x = (size.x - adjustedBorders.x - adjustedBorders.z) / pixelsPerUnit;
            float y = (size.y - adjustedBorders.y - adjustedBorders.w) / pixelsPerUnit;
            Vector2 tileSize = new(x, y);
            int num4 = 0;

            while (num < pixelAdjustedRect.xMax) {
                float num5 = num4 != CurrentSegment ? num3 : num3 * CurrentSegmentScale;
                float x2 = Mathf.Min(num + num5, pixelAdjustedRect.xMax);
                float x3 = num;
                Vector2 bottomLeft = PixelAdjustPoint(new Vector2(x3, pixelAdjustedRect.yMin));
                Vector2 topRight = PixelAdjustPoint(new Vector2(x2, pixelAdjustedRect.yMax));
                PaletteColorField paletteColorField = num4 >= UpgradedSegments ? num4 > CurrentSegment ? backgroundColor : upgradeColor : filledColor;

                AddBar(vh, num4 != CurrentSegment ? 1f : CurrentSegmentProgress, FromVector2(bottomLeft, topRight), pixelAdjustedRect, adjustedBorders, tileSize, outerUV, innerUV,
                    paletteColorField);

                num += num5 + num2;
                num4++;
            }
        }

        Vector4 GetAdjustedBorders(Vector4 border, Rect rect) {
            for (int i = 0; i <= 1; i++) {
                float num = border[i] + border[i + 2];

                if (rect.size[i] < (double)num && num != 0.0) {
                    float num2 = rect.size[i] / num;
                    border[i] *= num2;
                    border[i + 2] *= num2;
                }
            }

            return border;
        }

        void AddBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color) {
            if (Mathf.Abs(pos.x - rect.x) < 2f) {
                AddLeftBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            } else if (Mathf.Abs(pos.z - rect.xMax) < 2f) {
                AddRightBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            } else {
                AddMidBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            }
        }

        void AddLeftBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color) {
            float num = pos.y + border.y;
            float num2 = pos.w - border.w;
            float y = innerUVs.y;
            float num3 = (innerUVs.w - innerUVs.y) / tileSize.y;

            while (num < num2) {
                float num4 = num;
                float num5 = Mathf.Min(num4 + tileSize.y, num2);
                float num6 = y;
                float w = Mathf.Min(innerUVs.w, num6 + (num5 - num4) * num3);
                AddQuad(vh, new Vector4(pos.x, num4, pos.x + border.x, num5), new Vector4(outerUVs.x, num6, innerUVs.x, w), strokeColor);
                num = num5;
            }

            AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, strokeColor);

            if (fill < 1f) {
                num = pos.y + border.y;

                while (num < num2) {
                    float num7 = num;
                    float num8 = Mathf.Min(num7 + tileSize.y, num2);
                    float num9 = y;
                    float w2 = Mathf.Min(innerUVs.w, num9 + (num8 - num7) * num3);
                    AddQuad(vh, new Vector4(pos.z - border.z, num7, pos.z, num8), new Vector4(innerUVs.z, num9, outerUVs.z, w2), strokeColor);
                    num = num8;
                }
            }

            AddFill(vh, pos, border, border.x, 0f, fill, color);
        }

        void AddRightBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color) {
            float num = pos.y + border.y;
            float num2 = pos.w - border.w;
            float y = innerUVs.y;
            float num3 = (innerUVs.w - innerUVs.y) / tileSize.y;

            while (num < num2) {
                float num4 = num;
                float num5 = Mathf.Min(num4 + tileSize.y, num2);
                float num6 = y;
                float w = Mathf.Min(innerUVs.w, num6 + (num5 - num4) * num3);
                AddQuad(vh, new Vector4(pos.z - border.z, num4, pos.z, num5), new Vector4(innerUVs.z, num6, outerUVs.z, w), strokeColor);
                num = num5;
            }

            AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, strokeColor);
            AddFill(vh, pos, border, 0f, border.z, fill, color);
        }

        void AddTopAndBottomBorder(VertexHelper vh, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color) {
            float num = pos.x;
            float z = pos.z;
            float num2 = (innerUVs.z - innerUVs.x) / tileSize.x;
            float num3 = (pos.x - rect.xMin) * num2 % (tileSize.x * num2);

            while (num < z) {
                float num4 = num;
                float num5 = Mathf.Min(num + tileSize.x, z);
                float num6 = num3;
                float z2 = Mathf.Min(outerUVs.z, num6 + (num5 - num4) * num2);
                AddQuad(vh, new Vector4(num4, pos.y, num5, pos.y + border.y), new Vector4(num6, outerUVs.y, z2, innerUVs.y), color);
                AddQuad(vh, new Vector4(num4, pos.w - border.w, num5, pos.w), new Vector4(num6, innerUVs.w, z2, outerUVs.w), color);
                num = num5;
            }
        }

        void AddFill(VertexHelper vh, Vector4 pos, Vector4 border, float leftOffset, float rightOffset, float fill, Color color) {
            AddQuad(vh, new Vector4(pos.x + leftOffset, pos.y + border.y, pos.z - rightOffset - (1f - fill) * (pos.z - pos.x), pos.w - border.w), Vector4.zero, color);
        }

        void AddMidBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color) {
            float num = pos.y + border.y;
            float num2 = pos.w - border.w;
            float y = innerUVs.y;
            float num3 = (innerUVs.w - innerUVs.y) / tileSize.y;

            if (fill < 1f) {
                num = pos.y + border.y;

                while (num < num2) {
                    float num4 = num;
                    float num5 = Mathf.Min(num4 + tileSize.y, num2);
                    float num6 = y;
                    float w = Mathf.Min(innerUVs.w, num6 + (num5 - num4) * num3);
                    AddQuad(vh, new Vector4(pos.z - border.z, num4, pos.z, num5), new Vector4(innerUVs.z, num6, outerUVs.z, w), strokeColor);
                    num = num5;
                }
            }

            AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, strokeColor);
            AddFill(vh, pos, border, 0f, 0f, fill, color);
        }

        void AddQuad(VertexHelper vh, Vector4 pos, Vector4 uv, Color color) {
            int currentVertCount = vh.currentVertCount;
            vh.AddVert(new Vector2(pos.x, pos.y), color, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector2(pos.x, pos.w), color, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector2(pos.z, pos.w), color, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector2(pos.z, pos.y), color, new Vector2(uv.z, uv.y));
            vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
            vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
        }

        Vector4 FromVector2(Vector2 bottomLeft, Vector2 topRight) => new(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
    }
}
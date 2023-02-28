using System;
using UnityEngine;

[Serializable]
public class P3D_Brush {
    public static Action<Texture2D, P3D_Rect> OnPrePaint;

    public static Action<Texture2D, P3D_Rect> OnPostPaint;

    static Texture2D canvas;

    static int canvasW;

    static int canvasH;

    static P3D_Rect rect;

    static P3D_Matrix matrix;

    static P3D_Matrix inverse;

    static float opacity;

    static Color color;

    static Vector2 direction;

    static Texture2D shape;

    static Texture2D detail;

    static Vector2 detailScale;

    static P3D_Brush tempInstance;

    [Tooltip("The name of this brush (mainly used for saving/loading)")]
    public string Name = "Default";

    [Tooltip("The opacity of the brush (how solid it is)")] [Range(0f, 1f)]
    public float Opacity = 1f;

    [Tooltip("The angle of the brush in radians")] [Range(-(float)Math.PI, (float)Math.PI)]
    public float Angle;

    [Tooltip("The amount of pixels the brush gets moved from the pain location")]
    public Vector2 Offset;

    [Tooltip("The size of the brush in pixels")]
    public Vector2 Size = new(10f, 10f);

    [Tooltip("The blend mode of the brush")]
    public P3D_BlendMode Blend;

    [Tooltip("The shape of the brush")] public Texture2D Shape;

    [Tooltip("The color of the brush")] public Color Color = Color.white;

    [Tooltip("The normal direction of the brush (used for NormalBlend)")]
    public Vector2 Direction;

    [Tooltip("The detail texture when painting")]
    public Texture2D Detail;

    [Tooltip("The scale of the detail texture, allowing you to tile it")]
    public Vector2 DetailScale = new(0.5f, 0.5f);

    public static P3D_Brush TempInstance {
        get {
            if (tempInstance == null) {
                tempInstance = new P3D_Brush();
            }

            return tempInstance;
        }
    }

    public P3D_Brush GetTempClone() {
        CopyTo(TempInstance);
        return tempInstance;
    }

    public void CopyTo(P3D_Brush other) {
        if (other != null) {
            other.Name = Name;
            other.Opacity = Opacity;
            other.Angle = Angle;
            other.Offset = Offset;
            other.Size = Size;
            other.Blend = Blend;
            other.Color = Color;
            other.Direction = Direction;
            other.Shape = Shape;
            other.Detail = Detail;
            other.DetailScale = DetailScale;
        }
    }

    public void Paint(Texture2D newCanvas, P3D_Matrix newMatrix) {
        canvas = newCanvas;
        canvasW = newCanvas.width;
        canvasH = newCanvas.height;
        matrix = newMatrix;

        if (CalculateRect(ref rect)) {
            inverse = newMatrix.Inverse;
            opacity = Opacity;
            color = Color;
            direction = Direction;
            shape = Shape;
            detail = Detail;
            detailScale = DetailScale;

            if (OnPrePaint != null) {
                OnPrePaint(canvas, rect);
            }

            switch (Blend) {
                case P3D_BlendMode.AlphaBlend:
                    AlphaBlend.Paint();
                    break;

                case P3D_BlendMode.AlphaBlendRgb:
                    AlphaBlendRGB.Paint();
                    break;

                case P3D_BlendMode.AlphaErase:
                    AlphaErase.Paint();
                    break;

                case P3D_BlendMode.AdditiveBlend:
                    AdditiveBlend.Paint();
                    break;

                case P3D_BlendMode.SubtractiveBlend:
                    SubtractiveBlend.Paint();
                    break;

                case P3D_BlendMode.NormalBlend:
                    NormalBlend.Paint();
                    break;

                case P3D_BlendMode.Replace:
                    Replace.Paint();
                    break;
            }

            if (OnPostPaint != null) {
                OnPostPaint(canvas, rect);
            }
        }
    }

    bool CalculateRect(ref P3D_Rect rect) {
        Vector2 vector = matrix.MultiplyPoint(0f, 0f);
        Vector2 vector2 = matrix.MultiplyPoint(1f, 0f);
        Vector2 vector3 = matrix.MultiplyPoint(0f, 1f);
        Vector2 vector4 = matrix.MultiplyPoint(1f, 1f);
        float num = Mathf.Min(Mathf.Min(vector.x, vector2.x), Mathf.Min(vector3.x, vector4.x));
        float num2 = Mathf.Max(Mathf.Max(vector.x, vector2.x), Mathf.Max(vector3.x, vector4.x));
        float num3 = Mathf.Min(Mathf.Min(vector.y, vector2.y), Mathf.Min(vector3.y, vector4.y));
        float num4 = Mathf.Max(Mathf.Max(vector.y, vector2.y), Mathf.Max(vector3.y, vector4.y));

        if (num < num2 && num3 < num4) {
            rect.XMin = Mathf.Clamp(Mathf.FloorToInt(num), 0, canvasW);
            rect.XMax = Mathf.Clamp(Mathf.CeilToInt(num2), 0, canvasW);
            rect.YMin = Mathf.Clamp(Mathf.FloorToInt(num3), 0, canvasH);
            rect.YMax = Mathf.Clamp(Mathf.CeilToInt(num4), 0, canvasH);
            return true;
        }

        return false;
    }

    static bool IsInsideShape(P3D_Matrix inverseMatrix, int x, int y, ref Vector2 shapeCoord) {
        shapeCoord = inverseMatrix.MultiplyPoint(x, y);

        if (shapeCoord.x >= 0f && shapeCoord.x < 1f && shapeCoord.y >= 0f && shapeCoord.y < 1f) {
            return true;
        }

        return false;
    }

    static Color SampleRepeat(Texture2D texture, float u, float v) => texture.GetPixelBilinear(u % 1f, v % 1f);

    static class AdditiveBlend {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        Color color = P3D_Brush.color;

                        if (shape != null) {
                            color *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }

                        if (detail != null) {
                            color *= SampleRepeat(detail, num * i, num2 * j);
                        }

                        canvas.SetPixel(i, j, Blend(pixel, color));
                    }
                }
            }
        }

        static Color Blend(Color old, Color add) {
            old.r += add.r;
            old.g += add.g;
            old.b += add.b;
            old.a += add.a;
            return old;
        }
    }

    static class AlphaBlend {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        Color color = P3D_Brush.color;

                        if (shape != null) {
                            color *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }

                        if (detail != null) {
                            color *= SampleRepeat(detail, num * i, num2 * j);
                        }

                        canvas.SetPixel(i, j, Blend(pixel, color));
                    }
                }
            }
        }

        static Color Blend(Color old, Color add) {
            if (add.a > 0f) {
                float a = add.a;
                float num = 1f - a;
                float a2 = old.a;
                float num2 = a + a2 * num;
                old.r = (add.r * a + old.r * a2 * num) / num2;
                old.g = (add.g * a + old.g * a2 * num) / num2;
                old.b = (add.b * a + old.b * a2 * num) / num2;
                old.a = num2;
            }

            return old;
        }
    }

    static class AlphaBlendRGB {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        Color color = P3D_Brush.color;

                        if (shape != null) {
                            color *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }

                        if (detail != null) {
                            color *= SampleRepeat(detail, num * i, num2 * j);
                        }

                        canvas.SetPixel(i, j, Blend(pixel, color));
                    }
                }
            }
        }

        static Color Blend(Color old, Color add) {
            if (old.a > 0f && add.a > 0f) {
                float a = add.a;
                float num = 1f - a;
                float a2 = old.a;
                float num2 = a + a2 * num;
                old.r = (add.r * a + old.r * a2 * num) / num2;
                old.g = (add.g * a + old.g * a2 * num) / num2;
                old.b = (add.b * a + old.b * a2 * num) / num2;
            }

            return old;
        }
    }

    static class AlphaErase {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        float num3 = opacity;

                        if (shape != null) {
                            num3 *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                        }

                        if (detail != null) {
                            num3 *= SampleRepeat(detail, num * i, num2 * j).a;
                        }

                        canvas.SetPixel(i, j, Blend(pixel, num3));
                    }
                }
            }
        }

        static Color Blend(Color old, float sub) {
            old.a -= sub;
            return old;
        }
    }

    static class NormalBlend {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            if (shape != null && shape.format != TextureFormat.Alpha8) {
                for (int i = rect.XMin; i < rect.XMax; i++) {
                    for (int j = rect.YMin; j < rect.YMax; j++) {
                        if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                            Vector3 a = ColorToNormalXY(canvas.GetPixel(i, j));
                            Vector3 vector = ColorToNormalXY(shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y));

                            if (detail != null) {
                                Vector3 b = ColorToNormalXY(SampleRepeat(detail, num * i, num2 * j));
                                vector = CombineNormalsXY(vector, b);
                            }

                            a = CombineNormalsXY(a, vector, opacity);
                            a = ComputeZ(a);
                            a = Vector3.Normalize(a);
                            canvas.SetPixel(i, j, NormalToColor(a));
                        }
                    }
                }

                return;
            }

            for (int k = rect.XMin; k < rect.XMax; k++) {
                for (int l = rect.YMin; l < rect.YMax; l++) {
                    if (IsInsideShape(inverse, k, l, ref shapeCoord)) {
                        Vector3 a2 = ColorToNormalXY(canvas.GetPixel(k, l));
                        Vector2 vector2 = direction;
                        float num3 = opacity;

                        if (shape != null) {
                            num3 *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                        }

                        if (detail != null) {
                            Vector3 b2 = ColorToNormalXY(SampleRepeat(detail, num * k, num2 * l));
                            vector2 = CombineNormalsXY(vector2, b2);
                        }

                        a2 = CombineNormalsXY(a2, vector2, num3);
                        a2 = ComputeZ(a2);
                        a2 = Vector3.Normalize(a2);
                        canvas.SetPixel(k, l, NormalToColor(a2));
                    }
                }
            }
        }

        static Vector3 ColorToNormalXY(Color c) {
            Vector3 result = default;
            result.x = c.r * 2f - 1f;
            result.y = c.g * 2f - 1f;
            return result;
        }

        static Color NormalToColor(Vector3 n) {
            Color result = default;
            result.r = n.x * 0.5f + 0.5f;
            result.g = n.y * 0.5f + 0.5f;
            result.b = n.z * 0.5f + 0.5f;
            result.a = result.r;
            return result;
        }

        static Vector3 ComputeZ(Vector3 a) {
            a.z = Mathf.Sqrt(1f - a.x * a.x + a.y * a.y);
            return a;
        }

        static Vector3 CombineNormalsXY(Vector3 a, Vector3 b) {
            a.x += b.x;
            a.y += b.y;
            return a;
        }

        static Vector3 CombineNormalsXY(Vector3 a, Vector3 b, float c) {
            a.x += b.x * c;
            a.y += b.y * c;
            return a;
        }

        static Vector3 CombineNormalsXY(Vector3 a, Vector2 b, float c) {
            a.x += b.x * c;
            a.y += b.y * c;
            return a;
        }
    }

    static class Replace {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        Color color = P3D_Brush.color;
                        float num3 = opacity;

                        if (shape != null) {
                            num3 *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                        }

                        if (detail != null) {
                            color *= SampleRepeat(detail, num * i, num2 * j);
                        }

                        canvas.SetPixel(i, j, Color.Lerp(pixel, color, num3));
                    }
                }
            }
        }

        static Color Blend(Color old, Color add) {
            if (add.a > 0f) {
                float a = add.a;
                float num = 1f - a;
                float a2 = old.a;
                float num2 = a + a2 * num;
                old.r = (add.r * a + old.r * a2 * num) / num2;
                old.g = (add.g * a + old.g * a2 * num) / num2;
                old.b = (add.b * a + old.b * a2 * num) / num2;
                old.a = num2;
            }

            return old;
        }
    }

    static class SubtractiveBlend {
        public static void Paint() {
            Vector2 shapeCoord = default;
            float num = P3D_Helper.Reciprocal(canvasW * detailScale.x);
            float num2 = P3D_Helper.Reciprocal(canvasH * detailScale.y);
            color.a *= opacity;

            for (int i = rect.XMin; i < rect.XMax; i++) {
                for (int j = rect.YMin; j < rect.YMax; j++) {
                    if (IsInsideShape(inverse, i, j, ref shapeCoord)) {
                        Color pixel = canvas.GetPixel(i, j);
                        Color color = P3D_Brush.color;

                        if (shape != null) {
                            color *= shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }

                        if (detail != null) {
                            color *= SampleRepeat(detail, num * i, num2 * j);
                        }

                        canvas.SetPixel(i, j, Blend(pixel, color));
                    }
                }
            }
        }

        static Color Blend(Color old, Color sub) {
            old.r -= sub.r;
            old.g -= sub.g;
            old.b -= sub.b;
            old.a -= sub.a;
            return old;
        }
    }
}
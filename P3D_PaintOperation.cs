using UnityEngine;

public static class P3D_PaintOperation {
    static Color AlphaBlend(Color old, Color add) {
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

    static Color AlphaBlendRgb(Color old, Color add) {
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

    static Color AlphaErase(Color old, float sub) {
        old.a -= sub;
        return old;
    }

    static Color AdditiveBlend(Color old, Color add) {
        old.r += add.r;
        old.g += add.g;
        old.b += add.b;
        old.a += add.a;
        return old;
    }

    static Color SubtractiveBlend(Color old, Color sub) {
        old.r -= sub.r;
        old.g -= sub.g;
        old.b -= sub.b;
        old.a -= sub.a;
        return old;
    }

    static Color NormalBlend(Color old, Color add, float a) {
        float num = 1f - a;
        old.g = old.g * num + add.g * a;
        old.a = old.a * num + add.a * a;
        return old;
    }

    static Color NormalErase(Color old, float sub) => NormalBlend(old, new Color(0.5f, 0.5f, 1f, 0.5f), sub);

    static Color DirectionToColor(Vector2 direction) {
        Color result = default;
        result.r = direction.x * 0.5f + 0.5f;
        result.g = direction.y * 0.5f + 0.5f;
        result.b = Mathf.Sqrt(1f - direction.x * direction.x + direction.y * direction.y) * 0.5f + 0.5f;
        result.a = result.r;
        return result;
    }

    static void PrepareShape(Texture2D shape, ref int width) {
        if (shape != null) {
            int num = shape.width / shape.height;
            width *= num;
        }
    }

    public static P3D_Painter.PaintOperation AdditiveBlend(Color color, float opacity, int width, int height,
        Texture2D shape = null, Texture2D pattern = null, float patternScale = 1f, Texture2D texture = null,
        float textureScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                float num9 = num5 * textureScale;
                float num10 = num6 * textureScale;
                int num11 = num;
                int num12 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        Color add = color * opacity;

                        if (shape != null) {
                            add.a *= shape.GetPixelBilinear((j - num11) * num3, (i - num12) * num4).r;
                        }

                        if (pattern != null) {
                            add.a += add.a * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        if (texture != null) {
                            add *= texture.GetPixelBilinear(j * num9, i * num10);
                        }

                        pixel = AdditiveBlend(pixel, add);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation AlphaBlend(Color color, float opacity, int width, int height,
        Texture2D shape = null, Texture2D pattern = null, float patternScale = 1f, Texture2D texture = null,
        float textureScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                float num9 = num5 * textureScale;
                float num10 = num6 * textureScale;
                int num11 = num;
                int num12 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        Color add = color;
                        add.a *= opacity;

                        if (shape != null) {
                            add.a *= shape.GetPixelBilinear((j - num11) * num3, (i - num12) * num4).r;
                        }

                        if (pattern != null) {
                            add.a += add.a * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        if (texture != null) {
                            add *= texture.GetPixelBilinear(j * num9, i * num10);
                        }

                        pixel = AlphaBlend(pixel, add);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation AlphaBlendRgb(Color color, float opacity, int width, int height,
        Texture2D shape = null, Texture2D pattern = null, float patternScale = 1f, Texture2D texture = null,
        float textureScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                float num9 = num5 * textureScale;
                float num10 = num6 * textureScale;
                int num11 = num;
                int num12 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        Color add = color;
                        add.a *= opacity;

                        if (shape != null) {
                            add.a *= shape.GetPixelBilinear((j - num11) * num3, (i - num12) * num4).r;
                        }

                        if (pattern != null) {
                            add.a += add.a * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        if (texture != null) {
                            add *= texture.GetPixelBilinear(j * num9, i * num10);
                        }

                        pixel = AlphaBlendRgb(pixel, add);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation AlphaErase(float opacity, int width, int height, Texture2D shape = null,
        Texture2D pattern = null, float patternScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                int num9 = num;
                int num10 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        float num11 = opacity;

                        if (shape != null) {
                            num11 *= shape.GetPixelBilinear((j - num9) * num3, (i - num10) * num4).r;
                        }

                        if (pattern != null) {
                            num11 += num11 * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        pixel = AlphaErase(pixel, num11);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation NormalBlend(Vector2 direction, float opacity, int width, int height,
        Texture2D shape = null, Texture2D pattern = null, float patternScale = 1f, Texture2D texture = null,
        float textureScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                float num9 = num5 * textureScale;
                float num10 = num6 * textureScale;
                int num11 = num;
                int num12 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        Color add = DirectionToColor(direction);
                        float num13 = opacity;

                        if (shape != null) {
                            num13 *= shape.GetPixelBilinear((j - num11) * num3, (i - num12) * num4).r;
                        }

                        if (pattern != null) {
                            num13 *= 1f - num13 * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        if (texture != null) {
                            Color pixelBilinear = texture.GetPixelBilinear(j * num9, i * num10);
                            add.a = pixelBilinear.a + add.a - 0.5f;
                            add.g = pixelBilinear.g + add.g - 0.5f;
                        }

                        pixel = NormalBlend(pixel, add, num13);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation Preview(Mesh mesh, int submeshIndex, Transform transform, float opacity,
        int width, int height, Texture2D shape, Vector2 tiling, Vector2 offset) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                if (transform != null) {
                    float num = P3D_Helper.Reciprocal(canvas.width);
                    float num2 = P3D_Helper.Reciprocal(canvas.height);
                    float num3 = cenX * num;
                    float num4 = cenY * num2;
                    float x = width * num;
                    float y = height * num2;
                    float x2 = P3D_Helper.Reciprocal(width);
                    float y2 = P3D_Helper.Reciprocal(height);

                    if (width % 2 == 1) {
                        num3 += num * 0.5f;
                    }

                    if (height % 2 == 1) {
                        num4 += num2 * 0.5f;
                    }

                    P3D_BrushPreview.Show(mesh,
                        submeshIndex,
                        transform,
                        opacity,
                        new Vector2(x2, y2),
                        new Vector2(num3, num4),
                        new Vector2(x, y),
                        shape,
                        tiling,
                        offset);
                }
            };
        }

        return null;
    }

    public static P3D_Painter.PaintOperation SubtractiveBlend(Color color, float opacity, int width, int height,
        Texture2D shape = null, Texture2D pattern = null, float patternScale = 1f, Texture2D texture = null,
        float textureScale = 1f) {
        if (width > 0 && height > 0) {
            PrepareShape(shape, ref width);

            return delegate(Texture2D canvas, int cenX, int cenY) {
                int num = cenX - width / 2;
                int num2 = cenY - height / 2;
                int a = num + width;
                int a2 = num2 + height;
                float num3 = 1f / width;
                float num4 = 1f / height;
                float num5 = 1f / canvas.width;
                float num6 = 1f / canvas.height;
                float num7 = num5 * patternScale;
                float num8 = num6 * patternScale;
                float num9 = num5 * textureScale;
                float num10 = num6 * textureScale;
                int num11 = num;
                int num12 = num2;
                num = Mathf.Max(num, 0);
                num2 = Mathf.Max(num2, 0);
                a = Mathf.Min(a, canvas.width);
                a2 = Mathf.Min(a2, canvas.height);

                for (int i = num2; i < a2; i++) {
                    for (int j = num; j < a; j++) {
                        Color pixel = canvas.GetPixel(j, i);
                        Color sub = color * opacity;

                        if (shape != null) {
                            sub.a *= shape.GetPixelBilinear((j - num11) * num3, (i - num12) * num4).r;
                        }

                        if (pattern != null) {
                            sub.a += sub.a * pattern.GetPixelBilinear(j * num7, i * num8).r;
                        }

                        if (texture != null) {
                            sub *= texture.GetPixelBilinear(j * num9, i * num10);
                        }

                        pixel = SubtractiveBlend(pixel, sub);
                        canvas.SetPixel(j, i, pixel);
                    }
                }
            };
        }

        return null;
    }
}
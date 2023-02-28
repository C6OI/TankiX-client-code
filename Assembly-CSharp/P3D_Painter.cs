using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Painter {
    public bool Dirty;

    public Texture2D Canvas;

    public Vector2 Tiling = Vector2.one;

    public Vector2 Offset;

    public bool IsReady => Canvas != null;

    public void SetCanvas(GameObject gameObject, string textureName = "_MainTex", int newMaterialIndex = 0) {
        Material material = P3D_Helper.GetMaterial(gameObject, newMaterialIndex);

        if (material != null) {
            SetCanvas(material.GetTexture(textureName), material.GetTextureScale(textureName), material.GetTextureOffset(textureName));
        } else {
            SetCanvas(null, Vector2.zero, Vector2.zero);
        }
    }

    public void SetCanvas(Texture newTexture) {
        SetCanvas(newTexture, Vector2.one, Vector2.zero);
    }

    public void SetCanvas(Texture newTexture, Vector2 newTiling, Vector2 newOffset) {
        Texture2D texture2D = newTexture as Texture2D;

        if (texture2D != null && newTiling.x != 0f && newTiling.y != 0f) {
            if (!P3D_Helper.IsWritableFormat(texture2D.format)) {
                throw new Exception("Trying to paint a non-writable texture");
            }

            Canvas = texture2D;
            Tiling = newTiling;
            Offset = newOffset;
        } else {
            Canvas = null;
        }
    }

    public bool Paint(P3D_Brush brush, List<P3D_Result> results, P3D_CoordType coord = P3D_CoordType.UV1) {
        bool flag = false;

        if (results != null) {
            for (int i = 0; i < results.Count; i++) {
                flag |= Paint(brush, results[i], coord);
            }
        }

        return flag;
    }

    public bool Paint(P3D_Brush brush, P3D_Result result, P3D_CoordType coord = P3D_CoordType.UV1) {
        if (result != null) {
            return Paint(brush, result.GetUV(coord));
        }

        return false;
    }

    public bool Paint(P3D_Brush brush, Vector2 uv) {
        if (Canvas != null) {
            Vector2 vector = P3D_Helper.CalculatePixelFromCoord(uv, Tiling, Offset, Canvas.width, Canvas.height);
            return Paint(brush, vector.x, vector.y);
        }

        return false;
    }

    public bool Paint(P3D_Brush brush, float x, float y) {
        if (brush != null) {
            Vector2 vector = new(x, y);
            P3D_Matrix matrix = P3D_Helper.CreateMatrix(vector + brush.Offset, brush.Size, brush.Angle);
            return Paint(brush, matrix);
        }

        return false;
    }

    public bool Paint(P3D_Brush brush, P3D_Matrix matrix) {
        if (Canvas != null && brush != null) {
            brush.Paint(Canvas, matrix);
            Dirty = true;
            return true;
        }

        return false;
    }

    public void Apply() {
        if (Canvas != null && Dirty) {
            Dirty = false;
            Canvas.Apply();
        }
    }
}
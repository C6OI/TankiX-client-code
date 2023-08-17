using System;
using UnityEngine;

[Serializable]
public class P3D_Brush {
    public string Name = "Default";

    public P3D_BlendMode Blend;

    public Color Color = Color.white;

    public Vector2 Direction;

    public Texture2D Shape;

    public int Size = 10;

    public Texture2D Pattern;

    public float PatternScale = 5f;

    public Texture2D Texture;

    public float TextureScale = 5f;

    public Texture2D NormalTexture;

    public float NormalTextureScale = 5f;
}
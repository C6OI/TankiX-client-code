using System;
using UnityEngine;

[Serializable]
public struct P3D_GroupMask {
    [SerializeField] int mask;

    public P3D_GroupMask(int newMask) => mask = newMask;

    public static implicit operator int(P3D_GroupMask groupMask) => groupMask.mask;

    public static implicit operator P3D_GroupMask(int mask) => new(mask);
}
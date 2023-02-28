using System;
using UnityEngine;

[Serializable]
public struct P3D_Group {
    [SerializeField] int index;

    public P3D_Group(int newIndex) {
        if (newIndex <= 0) {
            index = 0;
        } else if (newIndex >= 31) {
            index = 31;
        } else {
            index = newIndex;
        }
    }

    public static implicit operator int(P3D_Group group) => group.index;

    public static implicit operator P3D_Group(int index) => new(index);
}
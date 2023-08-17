using Edelweiss.DecalSystem;
using UnityEngine;

public class DS_Decals : Decals {
    protected override DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject) =>
        a_GameObject.AddComponent<DS_DecalsMeshRenderer>();
}
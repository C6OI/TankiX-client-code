using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class TankContentLibrary : MonoBehaviour {
        public List<GameObject> weaponList;

        public List<GameObject> hullList;

        public List<ColoringComponent> coloringList;
    }
}
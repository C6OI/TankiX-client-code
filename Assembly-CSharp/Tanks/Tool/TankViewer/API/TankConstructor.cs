using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class TankConstructor : MonoBehaviour {
        ColoringComponent coloring;

        public GameObject HullInstance { get; private set; }

        public GameObject WeaponInstance { get; private set; }

        public void BuildTank(GameObject hull, GameObject weapon, ColoringComponent coloring) {
            CreateHull(hull);
            CreateWeapon(weapon);
            SetWeaponPosition();
            SetColoring(coloring);
        }

        void CreateWeapon(GameObject weapon) {
            WeaponInstance = Instantiate(weapon);
            WeaponInstance.transform.SetParent(transform, false);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        public void ChangeWeapon(GameObject weapon) {
            Destroy(WeaponInstance);
            CreateWeapon(weapon);
            SetWeaponPosition();
            SetColoring(coloring);
        }

        public void ChangeColoring(ColoringComponent coloring) {
            SetColoring(coloring);
        }

        public void ChangeHull(GameObject hull) {
            Destroy(HullInstance);
            CreateHull(hull);
            SetWeaponPosition();
            SetColoring(coloring);
        }

        void CreateHull(GameObject hull) {
            HullInstance = Instantiate(hull);
            HullInstance.transform.SetParent(transform, false);
            HullInstance.transform.localPosition = Vector3.zero;
            HullInstance.transform.localRotation = Quaternion.identity;
        }

        void SetWeaponPosition() {
            MountPointComponent component = HullInstance.GetComponent<MountPointComponent>();
            WeaponInstance.transform.position = component.MountPoint.position;
        }

        void SetColoring(ColoringComponent coloring) {
            this.coloring = coloring;
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetHullRenderer(HullInstance), coloring);
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetWeaponRenderer(WeaponInstance), coloring);
        }
    }
}
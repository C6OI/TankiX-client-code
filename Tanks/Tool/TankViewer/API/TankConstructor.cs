using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class TankConstructor : MonoBehaviour {
        public GameObject HullInstance { get; private set; }

        public GameObject WeaponInstance { get; private set; }

        public void BuildTank(GameObject hull, GameObject weapon, Texture2D coloring) {
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
        }

        public void ChangeColoring(Texture2D coloring) => SetColoring(coloring);

        public void ChangeHull(GameObject hull) {
            Destroy(HullInstance);
            CreateHull(hull);
            SetWeaponPosition();
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

        void SetColoring(Texture2D coloring) {
            TankMaterialsUtil.SetColoringTexture(TankBuilderUtil.GetHullRenderer(HullInstance), coloring);
            TankMaterialsUtil.SetColoringTexture(TankBuilderUtil.GetWeaponRenderer(WeaponInstance), coloring);
        }
    }
}
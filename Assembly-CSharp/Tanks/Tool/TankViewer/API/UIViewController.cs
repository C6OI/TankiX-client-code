using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API {
    public class UIViewController : MonoBehaviour {
        public Text hullName;

        public Text weaponName;

        public Text cameraTransform;

        public void ChangeHullName(string currentHullName) {
            hullName.text = currentHullName;
        }

        public void ChangeWeaponName(string currentWeaponName) {
            weaponName.text = currentWeaponName;
        }
    }
}
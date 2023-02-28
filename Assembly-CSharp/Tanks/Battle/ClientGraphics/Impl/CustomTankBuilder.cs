using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CustomTankBuilder : BehaviourComponent {
        [SerializeField] Transform tankContainer;

        [SerializeField] GameObject tankPrefab;

        [SerializeField] BattleResultTankCameraController camera;

        [SerializeField] Light light;

        void Awake() {
            light = GetComponentInChildren<Light>(true);
            light.transform.localPosition = new Vector3(14.1f, -4.98f, 2.73f);
            light.range = 150f;
            light.intensity = 2.5f;
            light.color = new Color32(244, 157, 27, byte.MaxValue);
            light.gameObject.SetActive(false);
        }

        public void ClearContainer() {
            light.gameObject.SetActive(false);
            tankContainer.DestroyChildren();
        }

        public void BuildTank(string hull, string weapon, string paint, string cover, bool bestPlayerScreen, RenderTexture newRenderTexture) {
            ClearContainer();
            light.gameObject.SetActive(true);
            camera.gameObject.SetActive(true);
            BattleResultsTankPositionComponent component = tankPrefab.GetComponent<BattleResultsTankPositionComponent>();
            component.hullGuid = hull;
            component.weaponGuid = weapon;
            component.paintGuid = paint;
            component.coverGuid = cover;
            GameObject gameObject = Instantiate(tankPrefab, tankContainer);
            gameObject.SetActive(true);

            if (bestPlayerScreen) {
                camera.SetupForBestPlayer();
            } else {
                camera.SetupForAwardScren();
            }

            camera.SetRenderTexture(newRenderTexture);
        }
    }
}
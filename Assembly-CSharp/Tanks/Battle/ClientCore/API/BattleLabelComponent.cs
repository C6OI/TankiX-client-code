using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635890692253926600L)]
    public class BattleLabelComponent : EntityBehaviour, Component {
        [SerializeField] long battleId;

        [SerializeField] GameObject map;

        [SerializeField] GameObject mode;

        [SerializeField] GameObject battleIcon;

        public long BattleId {
            get => battleId;
            set {
                battleId = value;
                gameObject.AddComponent<BattleLabelReadyComponent>();
            }
        }

        public string Map {
            get => map.GetComponent<Text>().text;
            set {
                map.GetComponent<Text>().text = value;
                map.SetActive(true);
            }
        }

        public string Mode {
            get => mode.GetComponent<Text>().text;
            set {
                mode.GetComponent<Text>().text = value;
                mode.SetActive(true);
            }
        }

        public bool BattleIconActivity {
            get => battleIcon.activeSelf;
            set => battleIcon.SetActive(value);
        }
    }
}
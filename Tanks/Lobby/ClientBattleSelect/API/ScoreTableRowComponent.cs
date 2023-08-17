using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635827527455935281L)]
    public class ScoreTableRowComponent : MonoBehaviour, Component {
        [SerializeField] RectTransform indicatorsContainer;

        [SerializeField] Text position;

        [SerializeField] Image background;

        readonly Dictionary<ScoreTableRowIndicator, ScoreTableRowIndicator> indicators = new();

        int positionNumber;

        public int Position {
            get => positionNumber;
            set {
                positionNumber = value;

                if (value == 0) {
                    position.text = string.Empty;
                    transform.SetAsLastSibling();
                } else {
                    position.text = value.ToString();
                    transform.SetSiblingIndex(positionNumber);
                }

                SetLayoutDirty();
            }
        }

        public Color Color {
            get => background.color;
            set => background.color = value;
        }

        public void SetLayoutDirty() => transform.parent.GetComponent<ScoreTableComponent>().SetDirty();

        public void AddIndicator(ScoreTableRowIndicator indicatorPrefab) {
            ScoreTableRowIndicator scoreTableRowIndicator = Instantiate(indicatorPrefab);
            indicators.Add(indicatorPrefab, scoreTableRowIndicator);
            scoreTableRowIndicator.transform.SetParent(indicatorsContainer, false);
            EntityBehaviour component = scoreTableRowIndicator.GetComponent<EntityBehaviour>();

            if (component != null) {
                component.BuildEntity(GetComponent<EntityBehaviour>().Entity);
            }

            Sort();
        }

        void Sort() {
            foreach (ScoreTableRowIndicator value in indicators.Values) {
                value.transform.SetSiblingIndex(value.index);
            }
        }

        public void RemoveIndicator(ScoreTableRowIndicator indicatorPrefab) {
            ScoreTableRowIndicator scoreTableRowIndicator = indicators[indicatorPrefab];
            Destroy(scoreTableRowIndicator.gameObject);
            indicators.Remove(indicatorPrefab);
        }

        public void HidePosition() => position.gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SelectCountryScreenComponent : BaseSelectScreenComponent {
        [SerializeField] Text rightPanelHint;

        [SerializeField] Text searchHint;

        public virtual string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        public virtual string SearchHint {
            set => searchHint.text = value;
        }
    }
}
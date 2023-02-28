using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class SmartConsoleActivator : MonoBehaviour, Component {
        [SerializeField] GameObject smartConsole;

        public GameObject SmartConsole {
            get => smartConsole;
            set => smartConsole = value;
        }
    }
}
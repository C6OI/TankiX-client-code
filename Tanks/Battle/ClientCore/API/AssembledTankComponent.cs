using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352905635226L)]
    public class AssembledTankComponent : Component {
        public AssembledTankComponent() { }

        public AssembledTankComponent(GameObject assemblyRoot) => AssemblyRoot = assemblyRoot;

        public GameObject AssemblyRoot { get; set; }
    }
}
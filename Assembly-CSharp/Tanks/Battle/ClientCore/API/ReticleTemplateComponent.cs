using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class ReticleTemplateComponent : Component {
        public TemplateDescription Template { get; set; }

        public string ConfigPath { get; set; }
    }
}
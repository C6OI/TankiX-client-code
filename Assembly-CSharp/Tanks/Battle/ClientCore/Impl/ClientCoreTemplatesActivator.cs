using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientCoreTemplatesActivator : DefaultActivator<AutoCompleting> {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        protected override void Activate() {
            TemplateRegistry.Register(typeof(BattleTemplate));
            TemplateRegistry.Register(typeof(DMTemplate));
            TemplateRegistry.Register(typeof(TeamBattleTemplate));
            TemplateRegistry.Register(typeof(TDMTemplate));
            TemplateRegistry.Register(typeof(CTFTemplate));
            TemplateRegistry.Register(typeof(TeamTemplate));
            TemplateRegistry.Register(typeof(FlagTemplate));
            TemplateRegistry.Register(typeof(PedestalTemplate));
            TemplateRegistry.Register<ServerShutdownNotificationTemplate>();
            TemplateRegistry.Register<BattleShutdownNotificationTemplate>();
            TemplateRegistry.Register<ServerShutdownTemplate>();
        }
    }
}
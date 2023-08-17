using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ClientCoreTemplatesActivator : DefaultActivator<AutoCompleting> {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        protected override void Activate() {
            TemplateRegistry.Register(typeof(SupplyBattleItemTemplate));
            TemplateRegistry.Register(typeof(RepairBattleItemTemplate));
            TemplateRegistry.Register(typeof(ArmorBattleItemTemplate));
            TemplateRegistry.Register(typeof(DamageBattleItemTemplate));
            TemplateRegistry.Register(typeof(SpeedBattleItemTemplate));
            TemplateRegistry.Register(typeof(MineBattleItemTemplate));
            TemplateRegistry.Register(typeof(MineTemplate));
            TemplateRegistry.Register(typeof(EffectTemplate));
            TemplateRegistry.Register(typeof(SpeedEffectTemplate));
            TemplateRegistry.Register(typeof(ArmorEffectTemplate));
            TemplateRegistry.Register(typeof(DamageEffectTemplate));
            TemplateRegistry.Register(typeof(RepairEffectTemplate));
            TemplateRegistry.Register(typeof(BurningEffectTemplate));
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
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ClientNetworkActivator : DefaultActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        [Inject] public static Protocol Protocol { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register(typeof(ClientSessionTemplate));
            ServerTimeServiceImpl service = new();
            ServiceRegistry.Current.RegisterService((ServerTimeService)service);
            ServiceRegistry.Current.RegisterService((ServerTimeServiceInternal)service);

            NetworkServiceImpl service2 =
                ECSNetworkServerBuilder.Build(EngineServiceInternal, Protocol, new SocketWrapper());

            ServiceRegistry.Current.RegisterService((NetworkService)service2);
        }

        protected override void Activate() { }
    }
}
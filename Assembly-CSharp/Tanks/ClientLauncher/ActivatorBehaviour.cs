using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lobby.ClientUserProfile.Impl;
using Platform.Common.ClientECSCommon.src.main.csharp.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientLogger.Impl;
using Platform.Library.ClientProtocol.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Platform.Library.ClientUnityIntegration.Impl;
using Platform.System.Data.Exchange.ClientNetwork.Impl;
using Platform.System.Data.Statics.ClientConfigurator.Impl;
using Platform.System.Data.Statics.ClientYaml.Impl;
using Platform.Tool.ClientUnityLogger.Impl;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientNavigation.Impl;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Tanks.ClientLauncher {
    public class ActivatorBehaviour : ClientActivator {
        readonly Type[] environmentActivators = new Type[11] {
            typeof(ClientLoggerActivator),
            typeof(ClientUnityLoggerActivator),
            typeof(CrashReporter),
            typeof(ClientProtocolActivator),
            typeof(YamlActivator),
            typeof(ConfigurationServiceActivator),
            typeof(EntitySystemActivator),
            typeof(ClientECSCommonActivator),
            typeof(ClientUserProfileActivator),
            typeof(ClientCoreTemplatesActivator),
            typeof(ClientUnityIntegrationActivator)
        };

        public void Awake() {
            UnityProfiler.Listen();
            SceneSwitcher.Clean();
            ActivateClient(MakeCoreActivators(), MakeNonCoreActivators());
        }

        void OnApplicationQuit() {
            SceneSwitcher.DisposeUrlLoaders();
            WWWLoader.DisposeActiveLoaders();
            SceneSwitcher.Clean();
            Process.GetCurrentProcess().Kill();
        }

        List<Activator> MakeCoreActivators() {
            return environmentActivators.Select(t => (Activator)System.Activator.CreateInstance(t)).ToList();
        }

        List<Activator> MakeNonCoreActivators() {
            Activator[] first = new Activator[1] {
                new ClientNetworkActivator()
            };

            return first.Concat(GetActivatorsAddedInUnityEditor()).ToList();
        }
    }
}
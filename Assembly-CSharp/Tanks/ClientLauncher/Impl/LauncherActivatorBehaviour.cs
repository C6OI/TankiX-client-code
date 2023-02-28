using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Common.ClientECSCommon.src.main.csharp.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientLogger.Impl;
using Platform.Library.ClientProtocol.Impl;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.Impl;
using Platform.System.Data.Statics.ClientConfigurator.Impl;
using Platform.System.Data.Statics.ClientYaml.Impl;
using Platform.Tool.ClientUnityLogger.Impl;
using Tanks.ClientLauncher.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherActivatorBehaviour : MonoBehaviour {
        readonly Type[] environmentActivators = new Type[7] {
            typeof(ClientLoggerActivator),
            typeof(ClientUnityLoggerActivator),
            typeof(ClientProtocolActivator),
            typeof(YamlActivator),
            typeof(ConfigurationServiceActivator),
            typeof(EntitySystemActivator),
            typeof(ClientECSCommonActivator)
        };

        public void Awake() {
            ProcessAdditionalClientCommands();

            if (ClientUpdater.IsUpdaterRunning()) {
                Application.Quit();
                return;
            }

            SceneSwitcher.Clean();

            if (!TryUpdateVersion()) {
                LaunchActivators();
            }
        }

        bool TryUpdateVersion() {
            if (ClientUpdater.IsApplicationRunInUpdateMode()) {
                ClientUpdater.Update();
                return true;
            }

            return false;
        }

        void LaunchActivators() {
            try {
                ActivatorsLauncher activatorsLauncher = new(environmentActivators.Select(t => (Activator)System.Activator.CreateInstance(t)));

                activatorsLauncher.LaunchAll(delegate {
                    new ActivatorsLauncher(GetActivatorsAddedInUnityEditor()).LaunchAll(OnAllActivatorsLaunched);
                });
            } catch (Exception ex) {
                LoggerProvider.GetLogger<LauncherActivatorBehaviour>().Error(ex.Message, ex);
                FatalErrorHandler.ShowFatalErrorScreen();
            }
        }

        void OnAllActivatorsLaunched() {
            gameObject.AddComponent<PreciseTimeBehaviour>();
            gameObject.AddComponent<EngineBehaviour>();
            GetComponent<LauncherBehaviour>().Launch();
        }

        IEnumerable<Activator> GetActivatorsAddedInUnityEditor() => from a in gameObject.GetComponentsInChildren<Activator>()
                                                                    where ((MonoBehaviour)a).enabled
                                                                    select a;

        void ProcessAdditionalClientCommands() {
            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
            string paramValue;

            if (commandLineParser.TryGetValue(LauncherConstants.CLEAN_PREFS, out paramValue)) {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
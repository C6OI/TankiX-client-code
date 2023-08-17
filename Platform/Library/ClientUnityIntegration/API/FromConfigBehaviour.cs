using System.Text.RegularExpressions;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Platform.Library.ClientUnityIntegration.API {
    public class FromConfigBehaviour : MonoBehaviour {
        static readonly Regex namespaceToConfigPath = new("(\\.Impl.*)|(\\.API.*)|(Client)", RegexOptions.IgnoreCase);

        static readonly Regex specialNames = new("(Component)|(Text)|(Texts)|(Localization)|(LocalizedStrings)",
            RegexOptions.IgnoreCase);

        static readonly INamingConvention naming = new PascalToCamelCaseNamingConvertion();

        [HideInInspector] [SerializeField] string configPath;

        [SerializeField] [HideInInspector] string yamlKey;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        public virtual string ConfigPath => !IsStaticConfigPath ? configPath : NamespaceToConfigPath();

        public virtual string YamlKey =>
            !IsStaticYamlKey ? yamlKey : naming.Apply(specialNames.Replace(GetType().Name, string.Empty));

        public virtual bool IsStaticYamlKey => true;

        public virtual bool IsStaticConfigPath => true;

        protected virtual void Awake() => GetValuesFromConfig();

        protected virtual string GetRelativeConfigPath() => string.Empty;

        string NamespaceToConfigPath() =>
            namespaceToConfigPath.Replace(GetType().Namespace, string.Empty).Replace(".", "/").ToLower() +
            GetRelativeConfigPath();

        void GetValuesFromConfig() {
            YamlNode childNode = ConfigurationService.GetConfig(ConfigPath).GetChildNode(YamlKey);
            UnityUtil.SetPropertiesFromYamlNode(this, childNode, new PascalToCamelCaseNamingConvertion());
        }
    }
}
using System;
using System.IO;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.Impl;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public class FileSystemConfigsImporter {
        readonly ConfigsMerger configsMerger = new();

        ConfigurationProfile configurationProfile;
        ConfigTreeNode root;

        string rootPath;

        [Inject] public static YamlService YamlService { get; set; }

        public T Import<T>(string path, ConfigurationProfile configurationProfile) where T : ConfigTreeNode, new() {
            this.configurationProfile = configurationProfile;
            root = new T();
            DirectoryInfo directoryInfo = new(path);
            rootPath = directoryInfo.FullName;
            CreateConfigTree(directoryInfo, configsMerger);
            configsMerger.Merge();
            return (T)root;
        }

        void CreateConfigTree(DirectoryInfo directoryInfo, ConfigsMerger merger) {
            DirectoryInfo[] directories = directoryInfo.GetDirectories();

            foreach (DirectoryInfo directoryInfo2 in directories) {
                string path = GetPath(directoryInfo2.FullName);
                root.FindOrCreateNode(path);
                CreateConfigTree(directoryInfo2, merger);
            }

            FileInfo[] files = directoryInfo.GetFiles();

            foreach (FileInfo fileInfo in files) {
                if (configurationProfile.Match(fileInfo.Name)) {
                    string path2 = GetPath(directoryInfo.FullName);
                    ConfigTreeNode configTreeNode = root.FindOrCreateNode(path2);

                    try {
                        YamlNodeImpl yamlNode = YamlService.Load(fileInfo);
                        merger.Put(configTreeNode, fileInfo.Name, yamlNode);
                    } catch (Exception innerException) {
                        throw new Exception(path2, innerException);
                    }
                }
            }
        }

        string GetPath(string fullConfigPath) => fullConfigPath.Substring(rootPath.Length).Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, '/');
    }
}
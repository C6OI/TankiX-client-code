using System;
using System.IO;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientConfigurator.Impl;
using Platform.System.Data.Statics.ClientYaml.API;
using Platform.System.Data.Statics.ClientYaml.Impl;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public class TarImporter {
        [Inject] public static YamlService YamlService { get; set; }

        public T ImportAll<T>(Stream inputStream) where T : ConfigTreeNode, new() => Import<T>(inputStream, AnyProfile.Instance);

        public T Import<T>(Stream inputStream, ConfigurationProfile configurationProfile) where T : ConfigTreeNode, new() {
            if (inputStream == null) {
                throw new ArgumentNullException("inputStream");
            }

            if (configurationProfile == null) {
                throw new ArgumentNullException("configurationProfile");
            }

            using (MemoryStream memoryStream = new()) {
                TransferTo(inputStream, memoryStream);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                return ReadToConfigTree<T>(memoryStream, configurationProfile);
            }
        }

        T ReadToConfigTree<T>(Stream inputStream, ConfigurationProfile configurationProfile) where T : ConfigTreeNode, new() {
            T result = new();
            ConfigsMerger configsMerger = new();
            string text = null;

            try {
                IReader reader = ReaderFactory.Open(inputStream);

                while (reader.MoveToNextEntry()) {
                    text = NormalizePath(reader.Entry.FilePath);

                    if (!string.IsNullOrEmpty(text)) {
                        if (reader.Entry.IsDirectory) {
                            result.FindOrCreateNode(text);
                        } else if (configurationProfile.Match(Path.GetFileName(text))) {
                            ConfigTreeNode configTreeNode = result.FindOrCreateNode(GetDirectoryName(text));
                            YamlNodeImpl yamlNode = LoadYaml(reader);
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
                            configsMerger.Put(configTreeNode, fileNameWithoutExtension, yamlNode);
                        }
                    }
                }
            } catch (Exception exception) {
                LoggerProvider.GetLogger(this).Fatal("Error read configs " + text, exception);
                throw;
            }

            configsMerger.Merge();
            return result;
        }

        static string NormalizePath(string path) => !path.StartsWith("./") ? path : path.Substring(2);

        string GetDirectoryName(string filePath) => Path.GetDirectoryName(filePath).Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, '/');

        YamlNodeImpl LoadYaml(IReader reader) {
            using (EntryStream source = reader.OpenEntryStream()) {
                using (MemoryStream memoryStream = new()) {
                    TransferTo(source, memoryStream);
                    memoryStream.Seek(0L, SeekOrigin.Begin);

                    using (StreamReader data = new(memoryStream)) {
                        return YamlService.Load(data);
                    }
                }
            }
        }

        void TransferTo(Stream source, Stream destination) {
            byte[] array = new byte[4096];
            int count;

            while ((count = source.Read(array, 0, array.Length)) != 0) {
                destination.Write(array, 0, count);
            }
        }

        class AnyProfile : ConfigurationProfile {
            public static readonly ConfigurationProfile Instance = new AnyProfile();

            public bool Match(string configName) => true;
        }
    }
}
using System;
using System.IO;
using Platform.System.Data.Statics.ClientYaml.Impl;
using YamlDotNet.Serialization;

namespace Platform.System.Data.Statics.ClientYaml.API {
    public interface YamlService {
        string Dump(object data);

        void Dump(object data, FileInfo file);

        T Load<T>(FileInfo file);

        YamlNodeImpl Load(FileInfo file);

        T Load<T>(string data);

        T Load<T>(YamlNodeImpl node);

        object Load(YamlNodeImpl node, Type type);

        object Load(string data, Type type);

        YamlNodeImpl Load(string data);

        YamlNodeImpl Load(TextReader data);

        T Load<T>(TextReader reader);

        void RegisterConverter(IYamlTypeConverter converter);
    }
}
using System;
using System.Collections.Generic;

namespace Platform.System.Data.Statics.ClientYaml.API {
    public interface YamlNode {
        YamlNode GetChildNode(string key);

        List<T> GetList<T>(string key);

        List<YamlNode> GetChildListNodes(string key);

        List<string> GetChildListValues(string key);

        string GetStringValue(string key);

        object GetValue(string key);

        bool HasValue(string key);

        T ConvertTo<T>();

        object ConvertTo(Type t);
    }
}
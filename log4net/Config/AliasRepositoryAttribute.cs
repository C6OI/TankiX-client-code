using System;

namespace log4net.Config {
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AliasRepositoryAttribute : Attribute {
        string m_name;

        public AliasRepositoryAttribute(string name) => Name = name;

        public string Name {
            get => m_name;
            set => m_name = value;
        }
    }
}
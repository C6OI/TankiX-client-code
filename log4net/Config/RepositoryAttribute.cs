using System;

namespace log4net.Config {
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly)]
    public class RepositoryAttribute : Attribute {
        string m_name;

        Type m_repositoryType;

        public RepositoryAttribute() { }

        public RepositoryAttribute(string name) => m_name = name;

        public string Name {
            get => m_name;
            set => m_name = value;
        }

        public Type RepositoryType {
            get => m_repositoryType;
            set => m_repositoryType = value;
        }
    }
}
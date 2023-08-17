using System;

namespace log4net.Config {
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    [Obsolete("Use AliasRepositoryAttribute instead of AliasDomainAttribute")]
    public sealed class AliasDomainAttribute : AliasRepositoryAttribute {
        public AliasDomainAttribute(string name)
            : base(name) { }
    }
}
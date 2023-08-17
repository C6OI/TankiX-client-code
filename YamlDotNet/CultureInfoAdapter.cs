using System;
using System.Globalization;

namespace YamlDotNet {
    sealed class CultureInfoAdapter : CultureInfo {
        readonly IFormatProvider _provider;

        public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider)
            : base(baseCulture.LCID) => _provider = provider;

        public override object GetFormat(Type formatType) => _provider.GetFormat(formatType);
    }
}
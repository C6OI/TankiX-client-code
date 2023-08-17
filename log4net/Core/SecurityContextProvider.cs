using log4net.Util;

namespace log4net.Core {
    public class SecurityContextProvider {
        protected SecurityContextProvider() { }

        public static SecurityContextProvider DefaultProvider { get; set; } = new();

        public virtual SecurityContext CreateSecurityContext(object consumer) => NullSecurityContext.Instance;
    }
}
using System.Collections;
using System.Collections.Specialized;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class TestContext {
        static readonly ThreadLocal<TestContext> context = new();
        readonly ThreadLocal<IDictionary> data = new();

        TestContext() {
            data.Set(new ListDictionary());
            SpyEntity = false;
        }

        public bool SpyEntity { get; set; }

        public static bool IsTestMode => context.Exists();

        public static TestContext Current => context.Get();

        public static void EnterTestMode() => context.Set(new TestContext());

        public void PutData(object key, object value) => data.Get().Add(key, value);

        public object GetData(object key) => data.Get()[key];

        public bool IsDataExists(object key) => data.Get().Contains(key);
    }
}
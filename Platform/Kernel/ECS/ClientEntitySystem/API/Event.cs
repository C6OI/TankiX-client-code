using System.Collections;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class Event {
        public override string ToString() {
            string text = GetType().Name + "[";
            PropertyInfo[] properties = GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties) {
                string text2 = text;
                text = text2 + propertyInfo.Name + "=" + ToString(propertyInfo.GetValue(this, null)) + ", ";
            }

            return text + "]";
        }

        string ToString(object obj) {
            if (obj is IEnumerable) {
                IEnumerable enumerable = (IEnumerable)obj;
                bool flag = true;
                string text = "[";

                foreach (object item in enumerable) {
                    if (!flag) {
                        text += ", ";
                    }

                    flag = false;
                    text += item;
                }

                return text + "]";
            }

            return obj.ToString();
        }
    }
}
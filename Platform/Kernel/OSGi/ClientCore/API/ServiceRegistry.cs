using System;
using System.Collections.Generic;
using System.Reflection;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class ServiceRegistry {
        static ServiceRegistry current;

        readonly Dictionary<Type, object> services = new();

        readonly Dictionary<Type, List<PropertyInfo>> waitingConsumers = new();

        static ServiceRegistry() => Reset();

        public static ServiceRegistry Current {
            get {
                if (current == null) {
                    throw new Exception("Service registry is not set");
                }

                return current;
            }
            set => current = value;
        }

        public static void Reset() => Current = new ServiceRegistry();

        public void RegisterService<T>(T service) {
            Type typeFromHandle = typeof(T);
            services[typeFromHandle] = service;

            if (waitingConsumers.ContainsKey(typeFromHandle)) {
                InjectIntoWaitingConsumers(typeFromHandle);
            }
        }

        void InjectIntoWaitingConsumers(Type type) {
            List<PropertyInfo> list = waitingConsumers[type];
            waitingConsumers.Remove(type);

            foreach (PropertyInfo item in list) {
                InjectIntoConsumer(item, services[type]);
            }
        }

        public void RegisterConsumer(PropertyInfo consumer) {
            if (!consumer.GetSetMethod(true).IsStatic) {
                string name = consumer.DeclaringType.Name;

                throw new ArgumentException(string.Format("Property {0}::{1} has to be static", name, consumer.Name),
                    "consumer");
            }

            Type propertyType = consumer.PropertyType;

            if (services.ContainsKey(propertyType)) {
                InjectIntoConsumer(consumer, services[propertyType]);
            } else {
                StoreWaitingConsumer(consumer, propertyType);
            }
        }

        void StoreWaitingConsumer(PropertyInfo consumer, Type type) {
            List<PropertyInfo> value;

            if (!waitingConsumers.TryGetValue(type, out value)) {
                value = new List<PropertyInfo>();
                waitingConsumers.Add(type, value);
            }

            value.Add(consumer);
        }

        void InjectIntoConsumer(PropertyInfo propertyInfo, object service) =>
            propertyInfo.GetSetMethod(true).Invoke(null, new object[1] { service });
    }
}
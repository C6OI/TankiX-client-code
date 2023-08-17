using System;
using System.Reflection;
using log4net;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using UnityEngine.SceneManagement;
using YamlDotNet.Serialization;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientUnityIntegration.API {
    public static class UnityUtil {
        static ILog log;

        public static void InheritAndEmplace(Transform child, Transform parent) {
            child.parent = parent;
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
        }

        public static void LoadScene(Object sceneAsset, string sceneAssetName) {
            GetLog().InfoFormat("LoadLevel {0}", sceneAssetName);
            SceneManager.LoadScene(sceneAssetName, LoadSceneMode.Single);
        }

        public static AsyncOperation LoadSceneAsync(Object sceneAsset, string sceneAssetName) {
            GetLog().InfoFormat("LoadSceneAsync {0}", sceneAssetName);
            return SceneManager.LoadSceneAsync(sceneAssetName, LoadSceneMode.Single);
        }

        public static void Destroy(Object obj) => Object.Destroy(obj);

        public static void DestroyChildren(this Transform root) {
            foreach (Transform item in root) {
                Object.Destroy(item.gameObject);
            }
        }

        public static void DestroyComponentsInChildren<T>(this GameObject go) where T : Component {
            T[] componentsInChildren = go.GetComponentsInChildren<T>(true);
            T[] array = componentsInChildren;

            foreach (T obj in array) {
                Object.Destroy(obj);
            }
        }

        static ILog GetLog() {
            if (log == null) {
                log = LoggerProvider.GetLogger(typeof(UnityUtil));
            }

            return log;
        }

        public static void SetPropertiesFromYamlNode(object target, YamlNode componentYamlNode,
            INamingConvention nameConvertor) {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;

            foreach (PropertyInfo propertyInfo in array) {
                string key = nameConvertor.Apply(propertyInfo.Name);

                if (componentYamlNode.HasValue(key) && propertyInfo.CanWrite) {
                    try {
                        propertyInfo.SetValue(target, componentYamlNode.GetValue(key), null);
                    } catch (ArgumentException) {
                        Debug.LogFormat("Can't convert to {0} from {1}",
                            propertyInfo.PropertyType,
                            componentYamlNode.GetValue(key).GetType());
                    }
                }
            }
        }
    }
}
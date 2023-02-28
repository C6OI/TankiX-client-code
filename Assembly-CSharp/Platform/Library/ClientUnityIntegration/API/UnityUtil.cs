using System;
using System.Collections;
using System.Diagnostics;
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

        public static void LoadScene(Object sceneAsset, string sceneAssetName, bool additive) {
            LoadSceneMode mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            GetLog().InfoFormat("LoadLevel {0}", sceneAssetName);
            SceneManager.LoadScene(sceneAssetName, mode);
        }

        public static AsyncOperation LoadSceneAsync(Object sceneAsset, string sceneAssetName) {
            GetLog().InfoFormat("LoadSceneAsync {0}", sceneAssetName);
            return SceneManager.LoadSceneAsync(sceneAssetName, LoadSceneMode.Single);
        }

        public static void Destroy(Object obj) {
            Object.Destroy(obj);
        }

        public static void DestroyChildren(this Transform root) {
            IEnumerator enumerator = root.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    Object.Destroy(transform.gameObject);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        public static void DestroyComponentsInChildren<T>(this GameObject go) where T : Component {
            T[] componentsInChildren = go.GetComponentsInChildren<T>(true);
            T[] array = componentsInChildren;

            foreach (T obj in array) {
                Object.Destroy(obj);
            }
        }

        public static string GetGameObjectPath(this GameObject obj) {
            string text = "/" + obj.name;

            while (obj.transform.parent != null) {
                obj = obj.transform.parent.gameObject;
                text = "/" + obj.name + text;
            }

            return text;
        }

        [Conditional("UNITY_EDITOR")]
        public static void Debug(this object obj) { }

        [Conditional("UNITY_EDITOR")]
        public static void Debug(this object obj, string message) {
            StackTrace stackTrace = new();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            UnityEngine.Debug.Log(string.Format("{0}: <i>{1}->{2}:</i> <b>{3}</b> {4}", Time.frameCount, method.ReflectedType.Name, method.Name, obj, message));
        }

        static ILog GetLog() {
            if (log == null) {
                log = LoggerProvider.GetLogger(typeof(UnityUtil));
            }

            return log;
        }

        public static void SetPropertiesFromYamlNode(object target, YamlNode componentYamlNode, INamingConvention nameConvertor) {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;

            foreach (PropertyInfo propertyInfo in array) {
                string key = nameConvertor.Apply(propertyInfo.Name);

                if (componentYamlNode.HasValue(key) && propertyInfo.CanWrite) {
                    try {
                        propertyInfo.SetValue(target, componentYamlNode.GetValue(key), null);
                    } catch (ArgumentException) {
                        UnityEngine.Debug.LogFormat("Can't convert to {0} from {1}", propertyInfo.PropertyType, componentYamlNode.GetValue(key).GetType());
                    }
                }
            }
        }
    }
}
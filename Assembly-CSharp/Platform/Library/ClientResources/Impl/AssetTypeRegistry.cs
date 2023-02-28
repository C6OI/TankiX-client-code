using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientResources.Impl {
    public static class AssetTypeRegistry {
        static readonly Dictionary<int, Type> hash2Type;

        static readonly Dictionary<string, Type> extension2Type;

        static AssetTypeRegistry() {
            hash2Type = new Dictionary<int, Type>();
            extension2Type = new Dictionary<string, Type>();
            RegisterAssetType<Object>();
            RegisterAssetType<Texture>();
            RegisterAssetType<Material>();
            RegisterAssetType<GameObject>();
            RegisterTypeExtension<Texture>(".png");
            RegisterTypeExtension<Texture>(".jpg");
            RegisterTypeExtension<Material>(".mat");
            RegisterTypeExtension<GameObject>(".fbx");
            RegisterTypeExtension<GameObject>(".prefab");
        }

        static void RegisterAssetType<T>() {
            RegisterAssetType(typeof(T));
        }

        static void RegisterAssetType(Type type) {
            hash2Type.Add(GetTypeHash(type), type);
        }

        static void RegisterTypeExtension<T>(string extension) {
            extension2Type.Add(extension.ToLower(), typeof(T));
        }

        public static int GetTypeHash(Type type) {
            if (type == typeof(Object)) {
                return 0;
            }

            return type.FullName.GetHashCode();
        }

        public static Type GetType(int hash) => hash2Type[hash];

        public static int GetTypeHashByExtension(string extension) => GetTypeHash(GetTypeByExtension(extension));

        public static Type GetTypeByExtension(string extension) {
            Type value;

            if (extension2Type.TryGetValue(extension.ToLower(), out value)) {
                return value;
            }

            return typeof(Object);
        }
    }
}
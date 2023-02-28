using System;
using LeopotamGroup.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeopotamGroup.Pooling {
    public sealed class PoolContainer : MonoBehaviour {
        [SerializeField] string _prefabPath = "UnknownPrefab";

        public GameObject CustomPrefab;

        [SerializeField] Transform _itemsRoot;

        readonly FastStack<IPoolObject> _store = new(32);

        Object _cachedAsset;

        Vector3 _cachedScale;

        bool _needToAddPoolObject;

        Type _overridedType;

        public string PrefabPath {
            get => _prefabPath;
            set => _prefabPath = value;
        }

        public Transform ItemsRoot {
            get => _itemsRoot;
            set => _itemsRoot = value;
        }

        bool LoadPrefab() {
            GameObject gameObject = !(CustomPrefab != null) ? Resources.Load<GameObject>(_prefabPath) : CustomPrefab;

            if (gameObject == null) {
                Debug.LogWarning("Cant load asset " + _prefabPath, this.gameObject);
                return false;
            }

            _cachedAsset = gameObject.GetComponent(typeof(IPoolObject));
            _needToAddPoolObject = (object)_cachedAsset == null;

            if (_needToAddPoolObject) {
                _cachedAsset = gameObject;
                _overridedType = typeof(PoolObject);
            } else if (_cachedAsset.GetType() != _overridedType) {
                Debug.LogWarning("Prefab already contains another IPoolObject-component", this.gameObject);
                return false;
            }

            _cachedScale = gameObject.transform.localScale;
            _store.UseCastToObjectComparer(true);

            if ((object)_itemsRoot == null) {
                _itemsRoot = transform;
            }

            return true;
        }

        public IPoolObject Get() {
            bool isNew;
            return Get(out isNew);
        }

        public IPoolObject Get(out bool isNew) {
            if ((object)_cachedAsset == null && !LoadPrefab()) {
                isNew = true;
                return null;
            }

            IPoolObject poolObject;

            if (_store.Count > 0) {
                poolObject = _store.Pop();
                isNew = false;
            } else {
                poolObject = !_needToAddPoolObject ? (IPoolObject)Instantiate(_cachedAsset) : (IPoolObject)((GameObject)Instantiate(_cachedAsset)).AddComponent(_overridedType);
                poolObject.PoolContainer = this;
                Transform poolTransform = poolObject.PoolTransform;

                if ((object)poolTransform != null) {
                    poolTransform.gameObject.SetActive(false);
                    poolTransform.SetParent(_itemsRoot, false);
                    poolTransform.localScale = _cachedScale;
                }

                isNew = true;
            }

            return poolObject;
        }

        public void Recycle(IPoolObject obj, bool checkForDoubleRecycle = true) {
            if (obj == null) {
                return;
            }

            Transform poolTransform = obj.PoolTransform;

            if ((object)poolTransform != null) {
                poolTransform.gameObject.SetActive(false);

                if ((object)poolTransform.parent != _itemsRoot) {
                    poolTransform.SetParent(_itemsRoot, true);
                }
            }

            if (!checkForDoubleRecycle || !_store.Contains(obj)) {
                _store.Push(obj);
            }
        }

        public static PoolContainer CreatePool<T>(string prefabPath, Transform itemsRoot = null) where T : IPoolObject => CreatePool(prefabPath, itemsRoot, typeof(T));

        public static PoolContainer CreatePool(string prefabPath, Transform itemsRoot = null, Type overridedType = null) {
            if (string.IsNullOrEmpty(prefabPath)) {
                return null;
            }

            if (overridedType != null && !typeof(IPoolObject).IsAssignableFrom(overridedType)) {
                Debug.LogWarningFormat("Invalid IPoolObject-type \"{0}\" for prefab \"{1}\"", overridedType, prefabPath);
                return null;
            }

            PoolContainer poolContainer = new GameObject().AddComponent<PoolContainer>();
            poolContainer._prefabPath = prefabPath;
            poolContainer._itemsRoot = itemsRoot;
            poolContainer._overridedType = overridedType;
            return poolContainer;
        }
    }
}
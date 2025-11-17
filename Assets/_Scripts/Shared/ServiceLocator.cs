using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    [CreateAssetMenu(fileName = "ServiceLocator", menuName = "Services/ServiceLocator")]
    public class ServiceLocator : ScriptableObject
    {
        private Dictionary<Type, object> cache = new();
        private Dictionary<string, GameObject> gameObjectsCache = new();
        private Dictionary<Type, List<Action<object>>> callbacks = new();
        private Dictionary<string, List<Action<GameObject>>> gameObjectCallbacks = new();

        private static ServiceLocator _instance;
        public static ServiceLocator Instance
        {
            get
            {
                //if (_instance == null)
                //{
                //    // Tenta carregar da pasta Resources
                //    _instance = Resources.Load<ServiceLocator>("ServiceLocator");
                    if (_instance == null)
                        Debug.LogError("[ServiceLocator] Nenhum ServiceLocator encontrado em Resources!");
                //}
                return _instance;
            }
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            _instance = Resources.Load<ServiceLocator>("ServiceLocator");
        }
        public T Get<T>() where T : class
        {
            if (cache.TryGetValue(typeof(T), out object obj))
            {
                if ((T)obj != null)
                    return (T)obj;
                cache.Remove(typeof(T));
            }
            var instance = UnityEngine.Object.FindAnyObjectByType(typeof(T)) as T;
            if (instance != null)
            {
                cache[typeof(T)] = instance;
                return instance;
            }
            Debug.LogWarning($"[ServiceLocator] Nenhum objeto do tipo {typeof(T).Name} encontrado!");
            return null;
        }
        /// <summary>
        /// Registers a callback to be invoked when an instance of type T becomes available. If an instance is already
        /// available, the callback is invoked immediately.
        /// </summary>
        /// <remarks>If an instance of type T is not currently available, the callback will be stored and
        /// invoked once the instance becomes available. Multiple callbacks for the same type are supported and will be
        /// invoked in the order they were registered.</remarks>
        /// <typeparam name="T">The type of the service to retrieve. Must be a reference type.</typeparam>
        /// <param name="onAvailableCallBack">The callback to execute when an instance of type T is available. The callback receives the instance as its
        /// parameter. Cannot be null.</param>
        public void GetAsync<T>(Action<T> onAvailableCallBack) where T : class
        {
            //Debug.Log($"[ServiceLocator] Instance hash: {GetHashCode()} | Method: GetAsync<{typeof(T).Name}>");
           // Debug.Log($"[{Time.time:F3}] GetAsync<{typeof(T).Name}> chamado.");
            if (cache.TryGetValue(typeof(T), out var instance) && instance != null)
            {
                onAvailableCallBack?.Invoke((T)instance);
                return;
            }
            if (!callbacks.ContainsKey(typeof(T)))
            {
                callbacks[typeof(T)] = new List<Action<object>>();
            }
            callbacks[typeof(T)].Add(m => onAvailableCallBack((T)m));
        }
        public GameObject Get(string name)
        {
            if (gameObjectsCache.TryGetValue(name, out GameObject obj))
                if (obj != null)
                    return obj;
            gameObjectsCache.Remove(name);
            GameObject instance = GameObject.Find(name);
            if (instance != null)
            {
                gameObjectsCache[name] = instance;
                return instance;
            }
            return null;
        }
        public void GetAsync(string name, Action<GameObject> onAvailableCallBack)
        {

            if (gameObjectsCache.TryGetValue(name, out GameObject gameObject) && gameObject != null)
            {
                onAvailableCallBack?.Invoke(gameObject);
                return;
            }
            if (!gameObjectCallbacks.ContainsKey(name))
                gameObjectCallbacks[name] = new List<Action<GameObject>>();
            gameObjectCallbacks[name].Add(m => onAvailableCallBack(m));
        }
        public void Register<T>(T obj) where T : class
        {
           // Debug.Log($"[ServiceLocator] Instance hash: {GetHashCode()} | Method: Register<{typeof(T).Name}>");
          //  Debug.Log($"[{Time.time:F3}] Register<{typeof(T).Name}> chamado.");
            if (obj == null)
                return;
            cache[typeof(T)] = obj;
            if (callbacks.TryGetValue(typeof(T), out var list))
            {
                foreach (var item in list)
                {
                    item?.Invoke(obj);
                }
                callbacks.Remove(typeof(T));
            }
        }
        public void Register(GameObject obj)
        {
            Register(obj.name, obj);
        }
        public void Register(string name, GameObject obj)
        {
            if (obj == null || string.IsNullOrEmpty(name))
                return;
            gameObjectsCache[name] = obj;
            if (gameObjectCallbacks.TryGetValue(name, out var list))
            {
                foreach(var item in list)
                {
                    item?.Invoke(obj);
                }
                gameObjectCallbacks.Remove(name);
            }

        }
        
        public void ClearCache() => cache.Clear();
        public void ClearGameObjectCache() => gameObjectsCache.Clear();

        public List<string> GetRegistredsServicesNames()
        {
            return cache.Keys.ToList().ConvertAll(t => t.Name);
        }
        public List<string> GetRegistredsGameObjectsNames()
        {
            return gameObjectsCache.Keys.ToList();
        }
        public void ResetService()
        {
            ClearCache();
            ClearGameObjectCache();
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    [CreateAssetMenu(fileName = "ServiceLocator", menuName = "Services/ServiceLocator")]
    public class ServiceLocator : ScriptableObject
    {
        private  Dictionary<Type, object> cache = new();
        private  Dictionary<string, GameObject> gameObjectsCache = new ();
        private static ServiceLocator _instance;
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Tenta carregar da pasta Resources
                    _instance = Resources.Load<ServiceLocator>("ServiceLocator");
                    if (_instance == null)
                        Debug.LogError("[ServiceLocator] Nenhum ServiceLocator encontrado em Resources!");
                }
                return _instance;
            }
        }
        public  T Get<T>() where T : class
        {
            if (cache.TryGetValue(typeof(T), out object obj))
            {
                if ((T)obj != null)
                    return (T)obj;
                cache.Remove(typeof(T));
            }
            var instance = UnityEngine.Object.FindAnyObjectByType(typeof(T)) as T;
            if(instance != null)
            {
                cache[typeof(T)] = instance;
                return instance;
            }
            Debug.LogWarning($"[ServiceLocator] Nenhum objeto do tipo {typeof(T).Name} encontrado!");
            return null;
        }
        public  GameObject Get(string name)
        {
            if (gameObjectsCache.TryGetValue(name, out GameObject obj))
               if(obj != null)
                    return obj;
            gameObjectsCache.Remove(name);
            GameObject instance = GameObject.Find(name);
            if(instance!=null)
            {
                gameObjectsCache[name] = instance;
                return instance;
            }
            return null;
        }
        public  void Register<T>(T obj) where T : class
        {
            if (obj == null) 
                return;
            cache[typeof(T)] = obj;
        }
        public  void Register(string name, GameObject obj)
        {
            if (obj != null && !string.IsNullOrEmpty(name))
                gameObjectsCache[name] = obj;
        }
        public  void ClearCache()
        {
            cache.Clear();
            gameObjectsCache.Clear();
        }
    


    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> cache = new();
        private static Dictionary<string, GameObject> gameObjectsCache = new ();
        public static T Get<T>() where T : class
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
        public static GameObject Get(string name)
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
        public static void Register<T>(T obj) where T : class
        {
            if (obj == null) 
                return;
            cache[typeof(T)] = obj;
        }
        public static void Register(string name, GameObject obj)
        {
            if (obj != null && !string.IsNullOrEmpty(name))
                gameObjectsCache[name] = obj;
        }
        public static void ClearCache() => cache.Clear();
    


    }
}

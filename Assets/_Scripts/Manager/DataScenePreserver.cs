using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-10)]
    public class DataScenePreserver : MonoBehaviour
    {
        public static DataScenePreserver Instance;
        private Dictionary<string, IDataHolder> holders = new();

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Set<T>(string key, T data) where T : class, new()
        {
            holders[key] = new DataHolder<T>(data);
        }

        public T Get<T>(string key) where T : class, new()
        {
            if (holders.TryGetValue(key, out var holder))
            {
                if (holder is DataHolder<T> typedHolder)
                {
                    return typedHolder.GetData();
                }

                throw new InvalidCastException($"O valor armazenado na chave '{key}' não é do tipo {typeof(T).Name}, mas sim {holder.DataHolderType.Name}.");
            }

            return default;
        }

        public bool TryGet<T>(string key, out T result) where T : class, new()
        {
            result = default;

            if (holders.TryGetValue(key, out var holder) && holder is DataHolder<T> typed)
            {
                result = typed.GetData();
                return true;
            }

            return false;
        }

        public bool Contains(string key) => holders.ContainsKey(key);

        public void Remove(string key) => holders.Remove(key);

        public void Clear() => holders.Clear();

    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace br.com.bonus630.thefrog.Debuggers
{
    public static class Watcher
    {

        // Guarda os valores antigos de cada variável
        private static readonly Dictionary<string, object> _lastValues = new();

        /// <summary>
        /// Verifica se o valor mudou. Se sim, loga automaticamente com tempo e frame.
        /// </summary>
        public static void Watch<T>(
            T value,
            MonoBehaviour context = null,
            [CallerMemberName] string varName = null)
        {
            string key = varName;

            if (_lastValues.TryGetValue(key, out object oldObj))
            {
                T oldValue = (T)oldObj;
                if (!Equals(oldValue, value))
                {
                    float time = Time.time;
                    int frame = Time.frameCount;
                    Debug.Log($"[Watcher] {varName} changed from {oldValue} → {value} at {time:F2}s (frame {frame})", context);
                    _lastValues[key] = value;
                }
            }
            else
            {
                // primeira vez que monitoramos
                _lastValues[key] = value;
            }
        }

        /// <summary>
        /// Remove a variável do monitoramento
        /// </summary>
        public static void Clear(string varName)
        {
            _lastValues.Remove(varName);
        }

        /// <summary>
        /// Limpa todos os watchers
        /// </summary>
        public static void ClearAll()
        {
            _lastValues.Clear();
        }
    }

}

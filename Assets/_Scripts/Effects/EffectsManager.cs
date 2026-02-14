using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;
        private List<IEffects> activeEffects = new();
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void Update()
        {
            float dt = Time.deltaTime;
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                activeEffects[i].UpdateEffects(dt);
                if (activeEffects[i].IsFinished)
                    activeEffects.RemoveAt(i);
            }
        }
        public ushort AddEffect(IEffects effect)
        {
            if (!activeEffects.Contains(effect))
            {
                ushort id;
                do
                {
                    id = (ushort)Random.Range(ushort.MinValue, ushort.MaxValue);
                }
                while (activeEffects.Any(r => r.ID == id));

                effect.ID = id;
                activeEffects.Add(effect);
                return id;
            }
            throw new System.Exception("Erro to add Effect in list");
        }
        public T GetEffect<T>(ushort ID) where T : IEffects
        {
            return activeEffects.FirstOrDefault(r => r is T && r.ID == ID) as T;
        }
    }
}

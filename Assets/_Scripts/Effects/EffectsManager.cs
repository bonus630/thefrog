using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;

        private List<IEffects> activeEffects = new List<IEffects>();

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

        public void AddEffect(IEffects effect)
        {
            if (!activeEffects.Contains(effect))
                activeEffects.Add(effect);
        }
    }
}

using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class Explosion : MonoBehaviour
    {
        [field: SerializeField] ScreenEffects screenEffects { get; set; }
        void Start()
        {
            screenEffects = ServiceLocator.Instance.Get<ScreenEffects>();
        }

        public void Explode()
        {
            if (screenEffects != null)
            {
                screenEffects.StartCameraShake(2, 2);
                screenEffects.GamepadShake(0.5f, 0.1f);
            }
        }
        public void Remove()
        {
            if (screenEffects != null)
            {
                screenEffects.GamepadShake();
                screenEffects.StopCameraShake();
            }
            Destroy(gameObject);
        }
    }
}

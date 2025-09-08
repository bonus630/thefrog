using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class Explosion : MonoBehaviour
    {
        ScreenEffects screenEffects;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            screenEffects = GameManager.Instance.ScreenEffects;
        }

        public void Explode()
        {
            Debug.Log("Explode screeneffects:" + screenEffects);
            screenEffects.StartCameraShake(2, 2);
            screenEffects.GamepadShake(0.5f, 0.1f);
        }
        public void Remove()
        {
            Debug.Log("Explosion remove");
            screenEffects.GamepadShake();
            screenEffects.StopCameraShake();
           
            Destroy(gameObject);
        }
    }
}

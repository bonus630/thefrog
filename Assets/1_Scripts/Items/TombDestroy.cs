using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class TombDestroy : IActivator
    {
        [SerializeField] AudioClip crackClip;
        [SerializeField] ParticleSystem effects;

        SpriteRenderer sr;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }


        public override void Activate()
        {
            sr.enabled = false;
            ServiceLocator.Instance.Get<AudioEffects>().Play(crackClip);
            ServiceLocator.Instance.Get<ScreenEffects>().ScreenAndGamepadShake(2);
            effects.Play();
            Destroy(gameObject, 0.1f);
        }

        public override void Deactive()
        {
            
        }
    }
}

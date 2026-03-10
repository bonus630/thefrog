using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class DisablePlayerInput : IActivator
    {
        IPlayer player;
        ScreenEffects sfx;

        public override void Activate()
        {
            player.AllInputsOn(false);
            sfx.FadeOut(0.2f);
        }

        public override void Deactive()
        {
            player.AllInputsOn(true);
            sfx.FadeIn(0.2f);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = ServiceLocator.Instance.Get<IPlayer>();
            sfx = ServiceLocator.Instance.Get<ScreenEffects>();
        }

      
    }
}

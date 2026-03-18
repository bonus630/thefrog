using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class ReturnInfinityWay : MonoBehaviour
    {
        bool running = false;
        public event System.Action OnTriggerEnterAction;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !running)
            {
                running = true;
                IPlayer p = ServiceLocator.Instance.Get<IPlayer>();
                p.AllInputsOn(false);
                ServiceLocator.Instance.Get<ScreenEffects>().FadeOut(1);
                p.FallsControl();
                if (!p.IsNormalGravity)
                    p.ChangeGravity(-1, 0.5f);
                OnTriggerEnterAction?.Invoke();
                collision.transform.position = new Vector3(350, 105, 0);
                ServiceLocator.Instance.Get<ScreenEffects>().FadeIn(1);
                p.AllInputsOn(true);

            }
        }
    }
}

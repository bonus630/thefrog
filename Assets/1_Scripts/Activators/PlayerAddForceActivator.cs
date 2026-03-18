using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PlayerAddForceActivator : MonoBehaviour
    {
        [SerializeField] AnimationCurve forceX;
        [SerializeField] AnimationCurve forceY;
        [SerializeField] float duration = 1f;
        [SerializeField] float multiplier = 10f;
        float time = 0;
        bool running = false;
        IPlayer player;
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IPlayer>(out player))
            {
                running = true;
                //player.AddForce(force);
                //player.AddForce(new Vector2(200,200));
                //gameObject.SetActive(false);
            }
        }
        private void FixedUpdate()
        {
            if (running)
            {
                time += Time.deltaTime;

                float normalTime = time / duration;

                if (normalTime > 1)
                {
                    running = false;
                    gameObject.SetActive(false);
                    return;
                }
                float x = forceX.Evaluate(normalTime);
                float y = forceY.Evaluate(normalTime);
                ServiceLocator.Instance.Get<IPlayer>()
                .AddForce(new Vector2(x * multiplier, y * multiplier), ForceMode2D.Force);

            }
        }
    }
}

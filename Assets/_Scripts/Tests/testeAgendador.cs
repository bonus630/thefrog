using UnityEngine;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;

namespace br.com.bonus630.tests
{
    public class testeAgendador : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("collision");
                ServiceLocator.Instance.LogRegistredsServicesNames();
                ServiceLocator.Instance.Get<IPlayer>().AddAction(new PlayerDirectorData(() =>
                ServiceLocator.Instance.Get<ScreenEffects>().FadeOut(), 1f));
                ServiceLocator.Instance.Get<IPlayer>().AddAction(new PlayerDirectorData(() =>
                { }, 10f));
                ServiceLocator.Instance.Get<IPlayer>().AddAction(new PlayerDirectorData(() =>
                ServiceLocator.Instance.Get<ScreenEffects>().FadeIn(), 1f));
                Destroy(gameObject);
            }
        }
    }
}

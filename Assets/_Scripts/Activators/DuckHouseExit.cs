using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class DuckHouseExit : MonoBehaviour
    {
        bool active = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player") && !active)
            {
                active = true;
                StartCoroutine(Teleport());
            }
        }

        private IEnumerator Teleport()
        {
            ScreenFader fader = FindAnyObjectByType<ScreenFader>();
            yield return fader.FadeOut();
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.GetPlayer.transform.position = new Vector3(76.37f, 32.68f, 0);
            yield return new WaitForSeconds(1f);
            yield return fader.FadeIn();
            yield return new WaitForSeconds(1f);
        }
    }
}

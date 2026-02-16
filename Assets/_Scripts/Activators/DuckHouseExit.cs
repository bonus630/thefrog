using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class DuckHouseExit : MonoBehaviour
    {
        [SerializeField] Vector3 ExitPosition;
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
            // yield return fader.FadeOut();
            yield return new WaitForSeconds(1f);
            FindAnyObjectByType<CameraBackground>().InitializeDayByHour(24);
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.GetPlayerScript.AddAction(new SchedulerData(() => { }, 3.5f, "wait"));
            GameManager.Instance.GetPlayerScript.AddAction(new SchedulerData(() => { Time.timeScale = 1f; }, 2f, "timescale"));
            GameManager.Instance.GetPlayerScript.AddAction(new SchedulerData(
                () => {
                    var se = ServiceLocator.Instance.Get<ScreenEffects>();
                    se.screenFader.FadeIn(1,false);
                        
                }, 1f, "fadein"));
            GameManager.Instance.GetPlayerScript.AddAction(new SchedulerData(() => { GameManager.Instance.GetPlayerScript.AllInputsOn(true, 0); }, 0f, "restoreinputs"));
            yield return null;
            GameManager.Instance.GetPlayer.transform.position = ExitPosition;
            //yield return new WaitForSeconds(1f);
            //yield return fader.FadeIn();
            //yield return new WaitForSeconds(1f);
        }
    }
}

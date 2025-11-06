using System.Collections;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class KoarStarts : TipsBase
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] CamerasController camerasController;
        [SerializeField] GameObject koarLimiter;
        bool start = false;
        int dialogueIndex = 0;
        IPlayer player;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IPlayer>(out player))
            {
                player.Alert();
                musicSource.Sleep();
                if (!start)
                    StartCoroutine(KoarStart());
                start = true;
            }
        }

        private IEnumerator KoarStart()
        {

            // GameManager.Instance.GetPlayerScript.MoveInputOn = false;
            GameManager.Instance.GetPlayerScript.AllInputsOn(false, 0);
             // yield return new WaitForSeconds(1f);
             yield return new WaitForSeconds(4f);
            yield return new WaitForEndOfFrame();
            //Time.timeScale = 0.5f;
            GameManager.Instance.GetPlayerScript.ChangeGravity(1f);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();
            Invoke(nameof(RestorePlayerInput), 3.5f);
            GameManager.Instance.GetPlayerScript.ChangeGravity(-1f);
            GameManager.Instance.PlayerStates.HasGravity = false;
            GameManager.Instance.PlayerStates.FallsControl = false;
            koarLimiter.SetActive(true);
            dialogueIndex++;
            yield return new WaitForSeconds(4.5f);//tempo de fala
            GameManager.Instance.EventCompleted(GameEventName.KoarFounded);
        }
        private void RestorePlayerInput()
        {
            Time.timeScale = 1f;
            GameManager.Instance.GetPlayerScript.AllInputsOn(true, 0);
            //GameManager.Instance.GetPlayerScript.MoveInputOn = true;
           // musicSource.Sleep();
            musicSource.InstantPlay(BackgroundMusic.DarkWind, true);
            // musicSource.CrossFade(BackgroundMusic.DarkWind, true);
            //Destroy(gameObject, 1f);
        }
        public override DialogueData GetDialogue(int index = -1)
        {
            return dialogues[dialogueIndex];
        }
    }
}

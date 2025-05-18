using System.Collections;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class KoarStarts : TipsBase
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] CamerasController camerasController;
        
        bool start = false;
        int dialogueIndex = 0;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player))
            {
                
                player.Alert();

                musicSource.Sleep();
                if(!start)
                    StartCoroutine(KoarStart());
                start = true;
            }
        }

        private IEnumerator KoarStart()
        {

            GameManager.Instance.GetPlayerScript.InputsOn = false;
            yield return new WaitForSeconds(4f);
            yield return new WaitForEndOfFrame();
            //Time.timeScale = 0.5f;
            GameManager.Instance.GetPlayerScript.ChangeGravity(1f);
            yield return new WaitForEndOfFrame();
            Invoke(nameof(RestorePlayerInput), 2f);
            GameManager.Instance.GetPlayerScript.ChangeGravity(-1f);
            GameManager.Instance.PlayerStates.HasGravity = false;
            GameManager.Instance.PlayerStates.FallsControl = false;
             dialogueIndex++;
            

        }
        private void RestorePlayerInput()
        {
            Time.timeScale = 1f;
            GameManager.Instance.GetPlayerScript.InputsOn = true;
            musicSource.Sleep();
            musicSource.CrossFade(BackgroundMusic.DarkWind);
            Destroy(gameObject,1f);
        }
        public override DialogueData GetDialogue(int index = -1)
        {
           return dialogues[dialogueIndex];
        }
    }
}

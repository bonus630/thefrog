using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCDuck : NPCBase, INPC
    {
       
        [SerializeField]AudioSource musicTarget;

        protected override void Awake()
        {
            base.Awake();
            this.CurrentDialogueData = dialogueData;

            CheckGameEvents();
        }

        private void CheckGameEvents()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.Gravity))
            {
                Dancing();
            }
        }
        public override void SetFinishDialogue()
        {
            StartCoroutine(SkyWalkerLearned());
        }
        private IEnumerator SkyWalkerLearned()
        {
            MusicSource musicSource;
            musicSource = GameObject.Find("AudioManager").GetComponent<MusicSource>();
            musicSource.StopAll();
            musicSource.Play(BackgroundMusic.Gravity);
            yield return new WaitForEndOfFrame();
            GameManager.Instance.EventCompleted(GameEventName.Gravity);
            GameManager.Instance.UpdatePlayer();
            GameManager.Instance.GetPlayerScript.ChangeGravity(1f, 5f);
            musicTarget.Play();
            GameManager.Instance.GetPlayerScript.InputsOn = false;
            yield return new WaitForEndOfFrame();   
            Invoke(nameof(RestorePlayerInput), 5f);
            Time.timeScale = 0.5f;
            FindAnyObjectByType<CameraBackground>().InitializeDayByHour(24);
        }
        private void RestorePlayerInput()
        {
            Time.timeScale = 1f;
            GameManager.Instance.GetPlayerScript.InputsOn = true;
        }
        public override void Interact()
        {

        }

        public override Transform GetTransform()
        {
            return transform;
        }
        public void Dancing()
        {
            GetComponent<Animator>().SetBool("Dancing", true);
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(transform.GetChild(0).gameObject);
        }

        public void CheckInitialDialogue(int dialogue)
        {
            throw new System.NotImplementedException();
        }
        protected override void OnGameEventCompleted(GameEvent gameEvent)
        {
            if (gameEvent.Name.Equals(GameEventName.Gravity))
                Dancing();
        }
    }
}

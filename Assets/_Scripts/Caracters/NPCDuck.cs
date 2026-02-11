using System;
using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCDuck : NPCBase, INPC
    {

        [SerializeField] AudioSource musicTarget;
        [SerializeField] GameObject teleporter;

        bool isFinishing = false;

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
        protected override void OnDisable()
        {
            Debug.Log("[NPCDuck] duck is disable");
            if(isFinishing)
            {
                GameManager.Instance.GetPlayerScript.AddAction(new PlayerDirectorData(() => { }, 3.5f,"wait"));
                GameManager.Instance.GetPlayerScript.AddAction(new PlayerDirectorData(() => { Time.timeScale = 1f; }, 2f,"timescale"));
                GameManager.Instance.GetPlayerScript.AddAction(new PlayerDirectorData(
                    () => ServiceLocator.Instance.Get<ScreenEffects>().FadeIn(), 1f,"fadein"));
                GameManager.Instance.GetPlayerScript.AddAction(new PlayerDirectorData(() => { GameManager.Instance.GetPlayerScript.AllInputsOn(true, 0); }, 0f,"restoreinputs"));
              
            }
            base.OnDisable();
        }
        private IEnumerator SkyWalkerLearned()
        {
            teleporter.SetActive(true);
            float time = Time.realtimeSinceStartup;
            MusicSource musicSource;
            musicSource = GameObject.Find("AudioManager").GetComponent<MusicSource>();
            musicSource.StopAll();
            musicTarget.Play();
            ScreenFader fader = FindAnyObjectByType<ScreenFader>();
            musicSource.Play(BackgroundMusic.Gravity);
            yield return new WaitForEndOfFrame();

            GameManager.Instance.EventCompleted(GameEventName.Gravity);
            GameManager.Instance.GetPlayerScript.UpdatePlayer();
            GameManager.Instance.GetPlayerScript.AllInputsOn(false, 0);
            isFinishing = true;
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(1f);
            //  yield return fader.FadeIn();
            GameManager.Instance.GetPlayerScript.ChangeGravity(1f, 0.2f);
           // Invoke(nameof(RestorePlayerInput), 3f);
            yield return fader.FadeOut();
            yield return new WaitForSeconds(1);
            // Debug.Log($"[NPCDuck] time: {(Time.realtimeSinceStartup - time):F3}");
            Destroy(gameObject);
        }
        //private IEnumerator SkyWalkerLearned()
        //{
        //    MusicSource musicSource;
        //    musicSource = GameObject.Find("AudioManager").GetComponent<MusicSource>();
        //    musicSource.StopAll();
        //    ScreenFader fader = FindAnyObjectByType<ScreenFader>();
        //    yield return fader.FadeOut();
        //    musicSource.Play(BackgroundMusic.Gravity);
        //    yield return new WaitForEndOfFrame();
        //    FindAnyObjectByType<CameraBackground>().InitializeDayByHour(24);
        //    GameManager.Instance.EventCompleted(GameEventName.Gravity);
        //    GameManager.Instance.GetPlayerScript.UpdatePlayer();
        //    yield return new WaitForSeconds(1.5f); 
        //    yield return fader.FadeIn();
        //    GameManager.Instance.GetPlayerScript.ChangeGravity(1f, 0.2f);
        //    musicTarget.Play();
        //    GameManager.Instance.GetPlayerScript.AllInputsOn(false, 0);
        //    yield return new WaitForEndOfFrame();   
        //    Invoke(nameof(RestorePlayerInput), 3f);
        //    Time.timeScale = 0.5f;

        //}
        private void RestorePlayerInput()
        {
            Time.timeScale = 1f;
            GameManager.Instance.GetPlayerScript.AllInputsOn(true, 0);
        }
        public override void Interact()
        {

        }
        private void FadeIn()
        {
            ServiceLocator.Instance.Get<ScreenFader>().FadeIn();
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

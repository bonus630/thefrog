using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.thefrog.Activators
{
    public class FactoryCutScene : IActivator
    {
        [field: SerializeField] public bool FirstStep { get; set; }
        [SerializeField] private ScreenEffects screenEffects;
        [SerializeField] MusicSource musicSource;
        //[SerializeField] IActivator toActive;
        //[SerializeField] IActivator toDisable;
        [SerializeField] IActivator Itens;
        [SerializeField] Tilemap foreground;
        [SerializeField] GameObject explosion;


        AudioSource AudioSource;
        BoxCollider2D col;

        private void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            col = GetComponent<BoxCollider2D>();
          
        }

        public override void Activate()
        {
            if (!FirstStep)
            {
                FirstStep = true;
                Debug.Log("s");
                StartCoroutine(FirstScene());
            }
            else
            {

            }
        }

        public override void Deactive()
        {
        }
        IEnumerator FirstScene()
        {
            screenEffects.StartCameraShake(2, 2);
            screenEffects.GamepadShake(0.5f, 0.1f);
            AudioSource.Play();
            screenEffects.FadeOut(1f);
            yield return new WaitForSeconds(1.5f);
            Itens.Activate();
            screenEffects.FadeIn(1f);
            screenEffects.StartCameraShake(1, 1);
            screenEffects.GamepadShake();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            Debug.Log("Collision factorycutscene");
            StartCoroutine(ForeGround());
            GameManager.Instance.PlayerStates.HasGravity = true;
            GameManager.Instance.PlayerStates.FallsControl = true;
            musicSource.Sleep();
        }
        private IEnumerator ForeGround()
        {
            float duration = 1f;
            float time = 0f;
            Color initialColor = foreground.color;
            Color endColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

            while (time < duration)
            {
                Color newColor = Color.Lerp(initialColor, endColor, time);
                time += Time.deltaTime;
                foreground.color = newColor;
                yield return null;
            }
            foreground.color = endColor;
        }
        bool timeOver = false;
        public void EndScene()
        {
            screenEffects.StopCameraShake();
            screenEffects.GamepadShake();
            StartCoroutine(EndSceneCoroutine()); 
            GameManager.Instance.StartTimer(15f, () => { timeOver = true; });
            //GameManager.Instance.TimeOverEvent += ;
        }
        IEnumerator EndSceneCoroutine()
        {
            System.Random rand = new System.Random();
            while (true)
            {
                GameObject explo = Instantiate(explosion, rand.Vector2FromRect(col.bounds), Quaternion.identity);
                float var = Random.Range(5, 10);
                explo.transform.localScale = new Vector3(var, var, 0);
                if (timeOver)
                    GameManager.Instance.GetPlayerScript.Hit();
                yield return new WaitForSeconds(1f);
                Destroy(explo,2f);
            }
            

        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}

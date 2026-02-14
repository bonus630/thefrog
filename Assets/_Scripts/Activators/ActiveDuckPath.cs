using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public sealed class ActiveDuckPath : MonoBehaviour
    {
        [SerializeField] StageBuilder stageBuilder;
        [SerializeField] GameObject cloud1;
        [SerializeField] GameObject cloud2;
        [SerializeField] GameObject cloud3;
        [SerializeField] MusicSource musicSource;
        [SerializeField] ScreenEffects screenEffects;
        bool build = false;
        bool enable = false;
        float time = 2;
        private Color transparent = new Color(1f, 1f, 1f, 0f);

        private void Start()
        {
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged  += ActiveDuckPath_HourChanged;
            screenEffects = ServiceLocator.Instance.Get<ScreenEffects>();
            musicSource = ServiceLocator.Instance.Get<MusicSource>();
            ActiveDuckPath_HourChanged(ServiceLocator.Instance.Get<IHourProvider>().Hour);
        }
        private void OnDisable()
        {
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged -= ActiveDuckPath_HourChanged;
        }
        private void ActiveDuckPath_HourChanged(int hour)
        {
            if (build)
                return;
            //Debug.Log($"[ActiveDuckPath][hourChanged] hour:{hour}");
            if (hour > 17 || hour < 6)
            {
                //cloud1.gameObject.SetActive(false);
                //cloud2.gameObject.SetActive(false);
                //cloud3.gameObject.SetActive(false);
                if(cloud1.GetComponent<SpriteRenderer>().color != transparent)
                    StartCoroutine(ChangeColor(Color.white, transparent));
                GetComponent<BoxCollider2D>().enabled = false;
            }
            if(hour >= 6 && hour <= 17)
            {
                //cloud1.gameObject.SetActive(true);
                //cloud2.gameObject.SetActive(true);
                //cloud3.gameObject.SetActive(true);
                if (cloud1.GetComponent<SpriteRenderer>().color != Color.white)
                    StartCoroutine(ChangeColor(transparent, Color.white));
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Builder home");
            if (!build)
            {
                build = true;
                GetComponent<AudioSource>().Play();
                GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>().enabled = false;
                StopAllCoroutines();
                StartCoroutine(Build());
            }
        }
        IEnumerator Build()
        {
            
            musicSource.StopAll();
            musicSource.InstantPlay(BackgroundMusic.DuckPath,false);
            float currentTime = 0;
            float gamepadShake = 0.2f;
            float x1 = cloud1.transform.position.x;
            float x2 = cloud2.transform.position.x;
            float x3 = cloud3.transform.position.x;
            stageBuilder.Build();
            screenEffects.GamepadShake(gamepadShake, 0);
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                cloud1.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud3.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud1.transform.position = new Vector3(Mathf.Lerp(x1, x1 + 6, currentTime), cloud1.transform.position.y, 0);
                cloud2.transform.position = new Vector3(Mathf.Lerp(x2, x2 + 6, currentTime), cloud2.transform.position.y, 0);
                cloud3.transform.position = new Vector3(Mathf.Lerp(x3, x3 + 6, currentTime), cloud3.transform.position.y, 0);

                screenEffects.GamepadShake(gamepadShake, 0);
                gamepadShake = Mathf.Lerp(0.2f,0,currentTime);
                yield return new WaitForSeconds(0.05f);
            }
            screenEffects.GamepadShake(0, 0);
            cloud1.GetComponent<SpriteRenderer>().color = transparent;
            cloud1.GetComponent<SpriteRenderer>().color = transparent;
            cloud1.GetComponent<SpriteRenderer>().color = transparent;

        }
        IEnumerator ChangeColor(Color start, Color end)
        {
            float currentTime = 0;
            while(currentTime <  time)
            {
                currentTime += Time.deltaTime;
                cloud1.GetComponent<SpriteRenderer>().color = Color.Lerp(start, end, currentTime);
                cloud2.GetComponent<SpriteRenderer>().color = Color.Lerp(start, end, currentTime);
                cloud3.GetComponent<SpriteRenderer>().color = Color.Lerp(start, end, currentTime);
                yield return new WaitForSeconds(0.05f);
            }
            cloud1.GetComponent<SpriteRenderer>().color = end;
            cloud1.GetComponent<SpriteRenderer>().color = end;
            cloud1.GetComponent<SpriteRenderer>().color = end;
        }
    }
}

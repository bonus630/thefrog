using System.Collections;
using br.com.bonus630.thefrog.Manager;
using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveDuckPath : MonoBehaviour
    {
        [SerializeField] StageBuilder stageBuilder;
        [SerializeField] GameObject cloud1;
        [SerializeField] GameObject cloud2;
        [SerializeField] GameObject cloud3;
        bool build = false;
        float time = 2;
        private Color transparent = new Color(1f, 1f, 1f, 0f);

        private void Start()
        {
            FindAnyObjectByType<CameraBackground>().HourChanged += ActiveDuckPath_HourChanged;
        }

        private void ActiveDuckPath_HourChanged(int hour)
        {
            if (hour > 17 || hour < 6)
            {
                cloud1.gameObject.SetActive(false);
                cloud2.gameObject.SetActive(false);
                cloud3.gameObject.SetActive(false);
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                cloud1.gameObject.SetActive(true);
                cloud2.gameObject.SetActive(true);
                cloud3.gameObject.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Builder home");
            if (!build)
            {
                build = true;
                GetComponent<AudioSource>().Play();
                GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>().enabled = false;
                StartCoroutine(Build());
            }
        }
        IEnumerator Build()
        {
            float currentTime = 0;
            float x1 = cloud1.transform.position.x;
            float x2 = cloud2.transform.position.x;
            float x3 = cloud3.transform.position.x;
            stageBuilder.Build();
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                cloud1.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud3.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, transparent, currentTime);
                cloud1.transform.position = new Vector3(Mathf.Lerp(x1, x1 + 6, currentTime), cloud1.transform.position.y, 0);
                cloud2.transform.position = new Vector3(Mathf.Lerp(x2, x2 + 6, currentTime), cloud2.transform.position.y, 0);
                cloud3.transform.position = new Vector3(Mathf.Lerp(x3, x3 + 6, currentTime), cloud3.transform.position.y, 0);

                yield return new WaitForSeconds(0.05f);
            }
            cloud1.GetComponent<SpriteRenderer>().color = transparent;
            cloud1.GetComponent<SpriteRenderer>().color = transparent;
            cloud1.GetComponent<SpriteRenderer>().color = transparent;
            GameObject.Find("Listener").GetComponent<MusicSource>().PlayFadIn(BackgroundMusic.DuckPath);

        }
    }
}

using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Lever : MonoBehaviour
    {

        [SerializeField] IActivator ItemToActive;
        [SerializeField] float delayTime;
        [SerializeField] bool actived = true;
        [SerializeField] string LevelID;
        GameObject prevCollision = null;
        int OnID = Animator.StringToHash("On");
        float timer = 1f;
        float time = 0;
        private void Start()
        {
            if(GameManager.Instance.IsActived(this.LevelID))
                SetActive(true,false);
        }
        private void Update()
        {
            time+= Time.deltaTime;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Lever collision:" + collision.gameObject.name);
            if (collision.gameObject == prevCollision && time < timer)
                return;
            time = 0f;
            prevCollision = collision.gameObject;
            SetActive(!actived);
        }

        IEnumerator TurnOff()
        {
            yield return new WaitForSeconds(delayTime);
            //SetActive(false);
            ItemToActive.Deactive();
        }
        IEnumerator TurnOn()
        {
            Debug.Log("Lever TurnOn");
            yield return new WaitForSeconds(delayTime);
            //SetActive(true);
            ItemToActive.Activate();
        }
        private void SetActive(bool actived,bool playAudio = true)
        {
            this.actived = actived;
            GetComponent<Animator>().SetBool(OnID, actived);
            GameManager.Instance.SetActived(this.LevelID, actived);
            if(playAudio)
                GetComponent<AudioSource>().Play();
            if (actived)
            {
                StartCoroutine(TurnOff());
            }
            else
            {
                StartCoroutine(TurnOn());
            }
        }
    }
}

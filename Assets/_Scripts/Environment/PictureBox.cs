using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class PictureBox : MonoBehaviour
    {
        [SerializeField] GameObject picture;
        [SerializeField] GameObject ghost;
        GameObject ghostInstance;
        int hour;

        float time = 0;
        bool spawning = false;
        IHourProvider hourProvider;
        void Start()
        {
            hourProvider = ServiceLocator.Instance.Get<IHourProvider>();
            hour = hourProvider.Hour;
            CheckHour(); 
        }
       
        void Update()
        {
            time += Time.deltaTime;
            if(time > 4)
            {
                time = 0;
                hour = hourProvider.Hour;
                CheckHour();
                if (spawning && ghost != null && ghostInstance == null)
                    ghostInstance = Instantiate(ghost,picture.transform.position,Quaternion.identity);
            }
        }
        private void CheckHour()
        {
            spawning = (hour > 19 || hour < 7);
            picture.SetActive(!spawning);
        }
    }
}

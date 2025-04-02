using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class AdventureStarts : MonoBehaviour
    {
        bool start = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                GameObject.Find("Listener").GetComponent<MusicSource>().PlayFadIn(BackgroundMusic.AdventureStarts);
                GameObject.Find("StartPointBuilder").GetComponent<StageBuilder>().Build();
            }
        }
    }
}

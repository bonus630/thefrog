using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class KiilPig : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !GameManager.Instance.IsEventCompleted(GameEventName.KillPig))
            {
                ExecuteKillPig();
            }
        }

        public void ExecuteKillPig()
        {
            FindAnyObjectByType<BossBattle>().EndBattle();
            FindAnyObjectByType<NPC_WallJump_Tutorial>().KillPig = true;
            GameObject.Find("Listener").GetComponent<MusicSource>().PlayFadIn(BackgroundMusic.PigIsDefead);
            GameManager.Instance.EventCompleted(GameEventName.KillPig);
            GameManager.Instance.UpdatePlayer();
            Destroy(GameObject.Find("BossActivator"));
        }

    }
}

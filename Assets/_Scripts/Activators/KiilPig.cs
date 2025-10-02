using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class KiilPig : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] IActivator fanToDisable;
        [SerializeField] IActivator fanToEnable;


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
            //FindAnyObjectByType<NPC_WallJump_Tutorial>().KillPig = true;
            musicSource.CrossFade(BackgroundMusic.PigIsDefead);
            GameManager.Instance.EventCompleted(GameEventName.KillPig);
            GameManager.Instance.GetPlayerScript.UpdatePlayer();
            Destroy(GameObject.Find("BossActivator"));
            fanToDisable.Deactive();
            fanToEnable.Activate();
        }

    }
}

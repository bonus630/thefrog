using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class KoarLimiter : MonoBehaviour
    {
        BoxCollider2D col;
        Transform playerPos;
        void Awake()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.DefeatWizard))
            {
                Destroy(gameObject);
                return;
            }
            col = GetComponent<BoxCollider2D>();
            playerPos = GameManager.Instance.GetPlayer.transform;
        }

        public void Update()
        {
            Change(col.OverlapPoint(playerPos.position));
        }
        private void Change(bool b)
        {
            Debug.Log("Koar limiter: " + !b);
            if(GameManager.Instance.IsEventCompleted(GameEventName.Gravity))
                GameManager.Instance.PlayerStates.HasGravity = !b;
            if(GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
                GameManager.Instance.PlayerStates.FallsControl = !b;
        }
    }
}

using br.com.bonus630.thefrog.Manager;
using UnityEditor.Search;
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
            GameManager.Instance.PlayerStates.HasGravity = !b;
            GameManager.Instance.PlayerStates.FallsControl = !b;
        }
    }
}

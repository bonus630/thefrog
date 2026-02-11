using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class TipsBase : MonoBehaviour, ITips
    {
        [SerializeField] protected List<DialogueData> dialogues;
        [SerializeField] protected bool autoPlay;
        [SerializeField] protected bool oneShot;
        [SerializeField] protected bool ciclily = true;
        [SerializeField] protected bool onLeaveDestroy = false;
        [SerializeField] protected GameEventName RemoveInCompleted = GameEventName.None;
        protected int count = 0;
        protected BoxCollider2D boxCollider;
        protected virtual void Awake()
        {
            if (RemoveInCompleted != GameEventName.None && GameManager.Instance.eventManager.AnyEventCompleted(RemoveInCompleted))
            {
                Destroy(gameObject);
                return;
            }
            boxCollider = GetComponent<BoxCollider2D>();
            GameManager.Instance.eventManager.GameEventCompleted += OnEventCompleted;
        }
        private void OnDisable()
        {
           // Debug.Log($"OnDestroy {name} | GameManager.Instance = {GameManager.Instance}");

            GameManager.Instance.eventManager.GameEventCompleted -= OnEventCompleted;
        }


        protected virtual void OnEventCompleted(GameEvent obj)
        {
            if (RemoveInCompleted != GameEventName.None && obj.Name == RemoveInCompleted)
                Destroy(gameObject);
        }
        public virtual DialogueData GetDialogue(int index = -1)
        {
            if (index > -1)
                return dialogues[index];
            else

                return dialogues[count];
        }
        public virtual void AutoPlayer(GameObject obj)
        {
            if (autoPlay)
            {
                IPlayer player;
                if (obj.TryGetComponent<IPlayer>(out player))
                {
                    player.ReadDialogue();
                    //  count++;
                    if (oneShot && !ciclily)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        private void OnDrawGizmos()
        {
            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider2D>();

            // Define a cor do gizmo
            Gizmos.color = Color.green;

            // Posição do gizmo = posição do objeto + offset do collider
            Vector3 colliderPos = (Vector2)transform.position + boxCollider.offset;

            // Ajusta para o layer do objeto, caso precise (por exemplo, se estiver com um parent com escala)
            Vector3 scale = transform.lossyScale;
            Vector3 size = new Vector3(boxCollider.size.x * scale.x, boxCollider.size.y * scale.y, 1f);

            // Desenha o wire cube no local correto
            Gizmos.DrawWireCube(colliderPos, size);
        }
        public bool HaveMore()
        {

            if (count >= dialogues.Count)
            {
                if (ciclily)
                {
                    count = 0;
                    return true;
                }
                return false;
            }
            return true;
        }
        //private void OnDisable()
        //{
        //    ServiceLocator.Instance.GetAsync<IPlayer>(p =>
        //        {
        //            if (p != null && ((MonoBehaviour)p).gameObject != null)
        //            {
        //                p.CancelDialogue();
        //            }
        //        });
        //}
        public void ReadTips()
        {

        }
    }
}

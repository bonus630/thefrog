using System.Collections.Generic;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.DialogueSystem;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class TipsBase : MonoBehaviour, ITips
    {
        [SerializeField] List<DialogueData> dialogues;
        [SerializeField] protected bool autoPlay;
        [SerializeField] protected bool oneShot;
        [SerializeField] protected bool ciclily = true;
        private int count = 0;
        private BoxCollider2D boxCollider;
        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        public DialogueData GetDialogue(int index = -1)
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
                Player player;
                if (obj.TryGetComponent<Player>(out player))
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

            // Posi��o do gizmo = posi��o do objeto + offset do collider
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

        public void ReadTips()
        {

        }
    }
}

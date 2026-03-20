using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class InputTiips : TipsBase
    {
        [SerializeField] string[] InputActionName;

        private float removeTime = 4f;

        protected override void Awake()
        {
            if (RemoveInCompleted != GameEventName.None && GameManager.Instance.eventManager.AnyEventCompleted(RemoveInCompleted))
            {
                Destroy(gameObject);
                return;
            }
        }

        public override DialogueData GetDialogue(int index = -1)
        {
            string r = ReplaceInput(dialogues[0].Dialogues[0].text, InputActionName);
            var dialogue = new Dialogue { Avatar = dialogues[0].Dialogues[0].Avatar, Name = dialogues[0].Dialogues[0].Name, text = r };
            var l = new List<Dialogue>();
            l.Add(dialogue);
            return new DialogueData() { Dialogues = l };
        }
        private string ReplaceInput(string text, string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                string spriteName = ServiceLocator.Instance.Get<IPlayer>().GetFormattedInputName(keys[i]);
                string spriteText = $"<sprite name=\"{spriteName}\">";
                text = text.Replace($"{{{keys[i]}}}", spriteText);
            }
            return text;
        }
        public override void AutoPlayer(GameObject obj)
        {
            if (autoPlay)
            {
                IPlayer player;
                if (obj.TryGetComponent<IPlayer>(out player))
                {
                    StopAllCoroutines();
                    removeTime = 4f;
                    player.ReadDialogue();
                    player.AllInputsOn(false, 0f, true, 2.1f);
                  
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                StartCoroutine(PrepareToRemove()); 
            }
        }
        private IEnumerator PrepareToRemove()
        {
            while(removeTime > 0)
            {
                removeTime -= Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }


}

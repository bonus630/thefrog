using System;
using System.Collections.Generic;
using UnityEngine;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    [Serializable]
    public struct Dialogue
    {
        public Sprite Avatar;
        public string Name;
        [TextArea(5, 10)]
        public string text;
    }

    [CreateAssetMenu(fileName = "Dialogues/Dialogue", menuName = "ScriptableObject/Dialogue", order = 1)]
    public class DialogueData : ScriptableObject
    {
        public string DialogueName;

        public List<Dialogue> Dialogues;

        public int Count { get { return Dialogues.Count; } }
        public bool IsReaded { get; set; }

    }
}

using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PasswordTip : TipsBase
    {
        public string[] ColorNames { get; private set; } = { "Vermelho","Azul","Verde","Amarelo" };
        public Color[] Colors { get; private set; } = { Color.red,Color.blue,Color.green,Color.yellow };
        public override DialogueData GetDialogue(int index = -1)
        {
            string texto = "Preciso lembrar!\n";
            List<int> password = GetComponent<PasswordGenerator>().password;
            for (int i = 0; i < password.Count; i++)
            {
                texto += ColorNames[password[i]] + " ";
            }
            texto += "\n\"oinc!\"";
            dialogues[0].Dialogues[0] = new Dialogue { Avatar = dialogues[0].Dialogues[0].Avatar, Name = dialogues[0].Dialogues[0].Name, text = texto };
            return dialogues[0];
            
        }
    }
    
}

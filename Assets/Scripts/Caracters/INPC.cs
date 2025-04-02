using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
namespace br.com.bonus630.thefrog.Caracters
{
    public interface INPC
    {
        DialogueData CurrentDialogueData { get; }
        bool HaveMoreDialogue();

        void SetFinishDialogue();
        void CheckInitialDialogue(int dialogue);

        Dictionary<string, string> GetDialogueVariables();
    }
}

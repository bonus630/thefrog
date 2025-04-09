using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
namespace br.com.bonus630.thefrog.Caracters
{
    public interface INPC
    {
        DialogueData CurrentDialogueData { get; }
        /// <summary>
        /// Retorna para o player verdadeiro caso o npc tenha mais falas
        /// </summary>
        /// <returns></returns>
        bool HaveMoreDialogue();
        /// <summary>
        /// O player executa esse metodo assim que não existe mais falas no dialogo
        /// </summary>
        void SetFinishDialogue();
        void CheckInitialDialogue(int dialogue);

        Dictionary<string, string> GetDialogueVariables();

        //void SetEventsCompleted();

    }
}

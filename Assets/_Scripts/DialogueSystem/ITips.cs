

using UnityEngine;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    public interface ITips
    {
        DialogueData GetDialogue(int index = -1);
        void ReadTips();
        bool HaveMore();
        void AutoPlayer(GameObject obj);
    }
}


using UnityEngine;
using UnityEngine.EventSystems;

namespace br.com.bonus630.thefrog.UI
{

    public class DebugSelectedUI : MonoBehaviour
    {
        void Update()
        {
            if (EventSystem.current != null)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;
                if (selected != null)
                {
                    Debug.Log("Botão selecionado atualmente: " + selected.name);
                }
                else
                {
                    Debug.Log("Nenhum botão selecionado no momento.");
                }
            }
        }
    }
}
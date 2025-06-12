using UnityEngine;
using UnityEditor;

namespace br.com.bonus630.thefrog
{
    public class HierarchyContextMenu
    {
        // Adiciona um item ao menu "GameObject > Custom > Create Point Holder"
        // O atalho "%#p" significa:
        //   %  = Ctrl (Cmd no macOS)
        //   #  = Shift
        //   p  = Tecla P
        // Logo, o atalho é: Ctrl + Shift + P
        //
        // Outros modificadores que você pode usar:
        //   %  = Ctrl (Cmd no macOS)
        //   #  = Shift
        //   &  = Alt
        //   _  = (sem modificador — só funciona em alguns casos específicos)

        [MenuItem("GameObject/Custom/Create Point Holder %#p", false, 10)]
        private static void CreatePointHolder(MenuCommand menuCommand)
        {
            // Cria um novo GameObject
            GameObject go = new GameObject("Point Holder");

            // Permite desfazer a criação com Ctrl+Z
            Undo.RegisterCreatedObjectUndo(go, "Create Point Holder");

            // Se houver um GameObject selecionado na Hierarquia, torna o novo objeto filho dele
            GameObject context = menuCommand.context as GameObject;
            if (context != null)
            {
                
                go.transform.SetParent(context.transform);
            }

            // Seleciona automaticamente o novo GameObject
            Selection.activeGameObject = go;
        }
    }
}

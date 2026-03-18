using UnityEngine;

namespace br.com.bonus630.thefrog.Debuggers
{
  
    public class UIDebugVisualizer : MonoBehaviour
    {
        public GameObject player;         // seu Player 3D
        public RectTransform uiElement;   // elemento UI do Canvas
        public Color insideColor = Color.green;
        public Color outsideColor = Color.red;

        private void OnGUI()
        {
            if (player == null || uiElement == null)
                return;

            Canvas canvas = uiElement.GetComponentInParent<Canvas>();
            if (canvas == null)
                return;

            Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            // fallback caso não tenha câmera
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && uiCamera == null)
                uiCamera = Camera.main;

            // posição do player em screen space
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, player.transform.position);

            // Inverter eixo Y do screenPos para o GUI
            screenPos.y = Screen.height - screenPos.y;

            // pega os cantos do UI e converte para screen space
            Vector3[] corners = new Vector3[4];
            uiElement.GetWorldCorners(corners);
            for (int i = 0; i < 4; i++)
            {
                Vector3 screenCorner = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[i]);
                corners[i] = new Vector3(screenCorner.x, Screen.height - screenCorner.y, 0);
            }

            // calcula retângulo do UI
            float xMin = corners[0].x;
            float yMin = corners[0].y;
            float width = corners[2].x - xMin;
            float height = corners[2].y - yMin;
            Rect uiRect = new Rect(xMin, yMin, width, height);

            // verifica se o player está dentro
            bool inside = uiRect.Contains(screenPos);

            // desenha retângulo da UI
            DrawRect(uiRect, Color.blue);

            // desenha posição do player
            DrawRect(new Rect(screenPos.x - 2, screenPos.y - 2, 4, 4), inside ? insideColor : outsideColor);

            // debug logs
            //Debug.Log($"ScreenPos Player: {screenPos}, Inside UI: {inside}");
        }

        // função auxiliar para desenhar retângulo na tela
        private void DrawRect(Rect rect, Color color)
        {
            Color oldColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = oldColor;
        }
    }

}

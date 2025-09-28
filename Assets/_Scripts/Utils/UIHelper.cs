using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class UIHelper 
    {
        public static bool IsGameObjectInsideUI(GameObject worldObject, RectTransform uiElement)
        {
            //if (uiElement == null)
            //    throw new System.Exception("uiElement is null");
            //if (worldObject == null)
            //    throw new System.Exception("worldObject is null");
            if (worldObject == null || uiElement == null)
                return false;

            // Posição do objeto em world space
            Vector3 worldPos = worldObject.transform.position;
           // Debug.Log($"WorldPos: {worldPos}");
          //  Debug.Log($"w:{uiElement.rect.width}");
            // Converte para coordenada de tela usando a câmera principal
            // Descobre a câmera correta do Canvas
            Canvas canvas = uiElement.GetComponentInParent<Canvas>();
            //if (canvas == null)
            //    throw new System.Exception("Canvas is null");
            Camera uiCamera = null;
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                uiCamera = canvas.worldCamera;
            }
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPos);
         //   Debug.Log($"Screen Point: {screenPoint}");
            //if (uiCamera == null)
            //    throw new System.Exception("Camera is null");
            // Verifica se o ponto da tela está dentro do UI
            return RectTransformUtility.RectangleContainsScreenPoint(uiElement, screenPoint, uiCamera);
        }
    }
}

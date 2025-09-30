using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class UIHelper 
    {
        public static bool IsGameObjectInsideUI(GameObject worldObject, RectTransform uiElement)
        {
            if (worldObject == null || uiElement == null)
                return false;
            Vector3 worldPos = worldObject.transform.position;
            Canvas canvas = uiElement.GetComponentInParent<Canvas>();
            Camera uiCamera = null;
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                uiCamera = canvas.worldCamera;
            }
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPos);
            return RectTransformUtility.RectangleContainsScreenPoint(uiElement, screenPoint, uiCamera);
        }
    }
}

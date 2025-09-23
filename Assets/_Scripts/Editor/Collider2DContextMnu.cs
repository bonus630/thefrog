using UnityEngine;
using UnityEditor;

public static class Collider2DContextMenu
{
    [MenuItem("CONTEXT/BoxCollider2D/ConverterToOverlap")]
    private static void ConvertToOverlap(MenuCommand command)
    {
        BoxCollider2D collider = command.context as BoxCollider2D;
        if (collider == null) return;

        Transform t = collider.transform;
        Vector2 center = t.TransformPoint(collider.offset);
        Vector2 size = new Vector2(collider.size.x * t.lossyScale.x, collider.size.y * t.lossyScale.y);
        float angle = t.eulerAngles.z;

        string code = $"Vector2 center = new Vector2({center.x}f, {center.y}f);\n" +
                      $"Vector2 size = new Vector2({size.x}f, {size.y}f);\n" +
                      $"float angle = {angle}f;\n" +
                      $"Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, enemyLayers);";

        Debug.Log("Código gerado:\n" + code);

        // Copia automaticamente para o clipboard usando GUIUtility
        GUIUtility.systemCopyBuffer = code;
        Debug.Log("Código copiado para o clipboard!");
    }
    [MenuItem("CONTEXT/CircleCollider2D/ConverterToOverlap")]
    private static void ConvertToOverlap2(MenuCommand command)
    {
        CircleCollider2D collider = command.context as CircleCollider2D;
        if (collider == null) return;

        Transform t = collider.transform;
        Vector2 center = t.TransformPoint(collider.offset);
        float radius = collider.radius;
        float angle = t.eulerAngles.z;

        string code = $"Vector2 center = new Vector2({center.x}f, {center.y}f);\n" +
                      $"float angle = {angle}f;\n" +
                      $"Collider2D[] hits = Physics2D.OverlapCircleAll(center, {radius}, layerMask);";

        Debug.Log("Código gerado:\n" + code);

        // Copia automaticamente para o clipboard usando GUIUtility
        GUIUtility.systemCopyBuffer = code;
        Debug.Log("Código copiado para o clipboard!");
    }
}

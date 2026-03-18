using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class CreateGrid : MonoBehaviour
    {
        [SerializeField] float startX;
        [SerializeField] float startY;
        [SerializeField] float cellWidth;
        [SerializeField] float cellHeight;
        [SerializeField] int rows;
        [SerializeField] int columns;
        [SerializeField] string prefixName;



        [ContextMenu("Create Grid")]
        public void Create()
        {
            startX = startX - cellWidth / 2;
            startY = startY + cellHeight / 2;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    GameObject go = new GameObject();
                    go.transform.SetParent(transform);
                    float posx = startX + j * cellWidth;
                    float posy = startY + i * cellHeight;
                    go.transform.localPosition = new Vector3(posx, posy, 0);
                    go.name = $"{prefixName} {posx} {posy}";
                    go.layer = 22;
                    go.AddComponent<BoxCollider2D>();
                    go.GetComponent<BoxCollider2D>().size = new Vector2(cellWidth, cellHeight);
                    go.GetComponent<BoxCollider2D>().isTrigger = true;
                    var relay = go.AddComponent<CollisionRelayEx>();
                    relay.index = i * columns + j;
                    relay.name = go.name;

                }
            }
        }
        [ContextMenu("Clear")]
        public void Clear()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}

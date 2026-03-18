using System;
using System.Collections;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.thefrog.Manager
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BuildSegment : MonoBehaviour
    {
        [SerializeField] ReturnInfinityWay top;
        [SerializeField] ReturnInfinityWay bottom;
        [SerializeField] float AnimationDurationTime = 1f;
        public Vector3 StartPoint { get; private set; }
        public Vector3 EndPoint { get; private set; }
        public int index { get; private set; }



        public event Action<bool, int> TriggerChanged;
        public event Action TriggerReset;

        public BoxCollider2D box;
        private readonly string STARTPOINT = "StartPoint";
        private readonly string ENDPOINT = "EndPoint";

        private GameObject module;

        private void Start()
        {
            box = GetComponent<BoxCollider2D>();
            top.OnTriggerEnterAction += Top_OnTriggerExitAction;

        }

        private void Top_OnTriggerExitAction()
        {
            TriggerReset?.Invoke();
        }

        public void Inflate(GameObject module, int index)
        {
            this.name = $"Segment {index}";
            this.index = index;
            this.module = Instantiate(module, transform);
            UpdatePoints();
            StartCoroutine(FitCollider());

        }
        private void UpdatePoints()
        {
            StartPoint = module.transform.Find(STARTPOINT).position;
            EndPoint = module.transform.Find(ENDPOINT).position;
        }
        public void SetPositionByStart(Vector3 position)
        {
            Vector3 destine = position + (transform.position - StartPoint);
            StartCoroutine(MoveTo(destine, AnimationDurationTime));

        }
        public void SetPositionByEnd(Vector3 position)
        {
            Vector3 destine = position + (transform.position - EndPoint);
            StartCoroutine(MoveTo(destine, AnimationDurationTime));
        }
        IEnumerator MoveTo(Vector3 target, float duration)
        {
            transform.position = target;
            UpdatePoints();
            transform.position = new Vector3(target.x, -10, 0);
            yield return null;
            float speed = Vector3.Distance(transform.position, target) / duration;
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    speed * Time.deltaTime
                );
                yield return null;
            }
            transform.position = target;
           
        }
        public bool InTheRight(Vector3 position) => transform.position.x >= position.x;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && ServiceLocator.Instance.Get<IPlayer>().BodyTouching(box))
            {
                TriggerChanged?.Invoke(true, index);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                TriggerChanged?.Invoke(false, index);
            }
        }
        private void OnDisable()
        {
            top.OnTriggerEnterAction -= Top_OnTriggerExitAction;

        }

        private IEnumerator FitCollider()
        {
            while (box == null)
                yield return null;
            float width = EndPoint.x - StartPoint.x;
            box.size = new Vector2(width, box.size.y);
        }
        private IEnumerator FitColliderByTiles()
        {
            while (box == null)
                yield return null;

            Tilemap tilemap;

            do
            {
                tilemap = GetComponentInChildren<Tilemap>();
                yield return null;
            }
            while (tilemap == null);

            while (tilemap.GetUsedTilesCount() == 0)
                yield return null;
            float width = EndPoint.x - StartPoint.x;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>(false);

            foreach (var sr in srs)
            {
                var b = sr.bounds;
                if (minY > b.min.y) minY = b.min.y;
                if (maxY < b.max.y) maxY = b.max.y;

            }
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos))
                    continue;

                Vector3 world = tilemap.GetCellCenterWorld(pos);

                if (world.y < minY) minY = world.y;
                if (world.y > maxY) maxY = world.y;
            }

            float half = tilemap.cellSize.y * 0.5f;

            minY -= half;
            maxY += half;

            float height = maxY - minY;
            box.size = new Vector2(width, height);
        }


    }
}

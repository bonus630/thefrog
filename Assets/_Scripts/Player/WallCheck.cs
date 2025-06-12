using UnityEngine;
namespace br.com.bonus630.thefrog.Player
{
    public class WallCheck : MonoBehaviour
    {
        [SerializeField] private Transform leftWallCheck;
        [SerializeField] private Transform rightWallCheck;
        [SerializeField] private Transform footerWallCheck;
        [SerializeField] private Vector2 size;
        [SerializeField] private LayerMask layerMask;



        public bool LeftWallCheck() => CheckWall(leftWallCheck.position, this.layerMask);
        public bool RightWallCheck() => CheckWall(rightWallCheck.position, this.layerMask);
        public bool LeftWallCheck(params string[] layerNames) => CheckWall(leftWallCheck.position, LayerMask.GetMask(layerNames));
        public bool RightWallCheck(params string[] layerNames) => CheckWall(rightWallCheck.position, LayerMask.GetMask(layerNames));

        public float RightDistance(Vector3 v) => Vector3.Distance(rightWallCheck.position, v);
        public float LeftDistance(Vector3 v) => Vector3.Distance(leftWallCheck.position, v);

        public bool IsFaceTo(Transform target) => RightDistance(target.position) < LeftDistance(target.position);


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(leftWallCheck.position, new Vector3(size.x, size.y, 0));
            Gizmos.DrawWireCube(rightWallCheck.position, new Vector3(size.x, size.y, 0));
            Gizmos.DrawWireCube(footerWallCheck.position, new Vector3(0.34f, 0.03f, 0));
        }

        private bool CheckWall(Vector2 side, LayerMask layer)
        {

            Collider2D coll = Physics2D.OverlapBox(side, size, 0, layer);
            if (coll != null)
            {
                // Debug.Log("Coll: "+coll.name);
                return true;
            }
            return false;
        }
        public bool CheckGround()
        {
            LayerMask layer = LayerMask.GetMask(new string[] { "Ground", "Platform", "StaticPlatforms" });
            Collider2D coll = Physics2D.OverlapBox(footerWallCheck.position,new Vector2(0.34f, 0.03f), 0, layer);
            if (coll != null)
            {
               // Debug.Log("Coll: "+coll.name);
                return true;
            }
            return false;
        }
    }

}

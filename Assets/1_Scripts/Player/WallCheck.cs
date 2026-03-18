using UnityEngine;
namespace br.com.bonus630.thefrog.Player
{
    public class WallCheck : MonoBehaviour
    {
        [SerializeField] private Transform leftWallCheck;
        [SerializeField] private Transform rightWallCheck;
        [SerializeField] private Transform footerWallCheck;
        [SerializeField] private Transform headWallCheck;
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
            Gizmos.DrawLine(footerWallCheck.position, new Vector3(footerWallCheck.position.x, footerWallCheck.position.y - 1 * 0.5f,1));
            Gizmos.DrawLine(headWallCheck.position, new Vector3(headWallCheck.position.x, headWallCheck.position.y - 1 * 0.5f,1));
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
        public bool CheckGround() => Check(footerWallCheck.position);
        public bool CheckRoof() => Check(headWallCheck.position);
     
        private bool Check(Vector3 pos)
        {
            LayerMask layer = LayerMask.GetMask(new string[] { "Ground", "Platform", "StaticPlatforms" });
            Collider2D coll = Physics2D.OverlapBox(pos, new Vector2(0.34f, 0.03f), 0, layer);
            if (coll != null)
            {

                // Debug.Log("Coll: "+coll.);
                return true;
            }
            return false;
        }
        public bool NearGround(out float groundAngle,float direction = -1)
        {
            LayerMask layer = LayerMask.GetMask(new string[] { "Ground", "Platform", "StaticPlatforms" });
            var r = Physics2D.Linecast(footerWallCheck.position, new Vector2(footerWallCheck.position.x, footerWallCheck.position.y  + direction * 0.5f),layer);
            
            if (r.collider != null)
            {
                float angle = Mathf.Atan2(r.normal.x, r.normal.y) * Mathf.Rad2Deg;
                groundAngle = angle - 90; 
                return true;
            }
            groundAngle = 0;
            return false;
        }
    }

}

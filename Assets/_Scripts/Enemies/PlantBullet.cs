using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class PlantBullet : MonoBehaviour
    {
        [SerializeField] float shootForce = 1000;

        [SerializeField][Range(-1, 1)] int direction = -1;

        public int Direction { get { return direction; } set { direction = value; } }


        void Start()
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootForce * direction, ForceMode2D.Impulse);
        }


    }
}

using br.com.bonus630.thefrog.Items;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] Vector3 direction;
        [SerializeField] GameObject bullet;
        [SerializeField] float timeInterval = 2f;
        float timer = 0;

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeInterval)
            {
                timer = 0;
                var b = Instantiate(bullet, transform);
                if (b != null)
                    b.GetComponent<Fireball>().Launch(direction);
            }
        }
    }
}

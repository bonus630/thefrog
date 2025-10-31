using System.Collections;
using UnityEngine;

namespace br.com.bonus630.thefrog
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RandomRotation : MonoBehaviour
    {
        [SerializeField] float range = 0.1f;
        [SerializeField] float delay = 0.5f;
        float rotation = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            rotation = Random.Range(-range, range);
            GetComponent<Rigidbody2D>().AddTorque(rotation);
        }
        
    }
}

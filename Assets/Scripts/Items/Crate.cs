using System.Collections;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Crate : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(Break());
        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator Break()
        {
            for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
            {
                Debug.Log(i);
                transform.GetChild(0).transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 200));
                yield return null;
            }
        }
    }
}

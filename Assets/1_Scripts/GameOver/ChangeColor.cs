using UnityEngine;
namespace br.com.bonus630.thefrog.GameOver
{
    public class ChangeColor : MonoBehaviour
    {

        void Start()
        {
            Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            gameObject.GetComponent<Camera>().backgroundColor = color;
        }


    }
}

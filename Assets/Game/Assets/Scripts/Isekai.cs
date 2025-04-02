using System.Collections;
using UnityEngine;

public class Isekai : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(MiniGameManager.Instance.lives == 1)
            {
                transform.GetChild(0).gameObject.GetComponent<MoveCycle>().speed = 100f;
                StartCoroutine(NormalSpeed());
            }
        }
    }
    IEnumerator NormalSpeed()
    {
        yield return new WaitForSeconds(0.4f);
        transform.GetChild(0).gameObject.GetComponent<MoveCycle>().speed = 1f;
    }
}

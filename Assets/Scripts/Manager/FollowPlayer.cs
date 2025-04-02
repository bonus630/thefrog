using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //[SerializeField] private float yOffset = -0.6f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = player.transform.position.x;
        if(player.transform.position.x < -3)
            x = -3;
        if(player.transform.position.x > 38)
            x = 38;
        float y = gameObject.transform.position.y;
        float z = gameObject.transform.position.z;
        Vector3 pos = new Vector3(x, y, z);
        gameObject.transform.position = pos;
    }
}

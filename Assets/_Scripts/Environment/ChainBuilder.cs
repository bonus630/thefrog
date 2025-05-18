using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class ChainBuilder : MonoBehaviour
    {
        public GameObject linkFrontPrefab;
        public GameObject linkSidePrefab;
        public int linkCount = 12;
        public Transform startPoint;
        public Transform endPoint;  

        void Start()
        {
            GameObject previousLink = null;

            for (int i = 0; i < linkCount; i++)
            {
                GameObject prefabToUse = (i % 2 == 0) ? linkFrontPrefab : linkSidePrefab;
                Vector3 position = startPoint.position - new Vector3(0, i * 0.15f, 0);
                GameObject link = Instantiate(prefabToUse, position, Quaternion.identity);
                link.transform.SetParent(startPoint, worldPositionStays: true);
                link.name = i.ToString();
                HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
                if (i == 0)
                {
                    joint.connectedBody = startPoint.GetComponent<Rigidbody2D>();
                }
                else
                {
                    joint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
                }

                previousLink = link;
            }
            endPoint.GetComponent<HingeJoint2D>().connectedBody = previousLink.GetComponent<Rigidbody2D>();

        }
    }

}

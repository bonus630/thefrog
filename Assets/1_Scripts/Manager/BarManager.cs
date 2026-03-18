using br.com.bonus630.thefrog.Shared;

using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class BarManager : MonoBehaviour
    {
        List<GameObject> bars = new List<GameObject>();
        [SerializeField] GameObject bar;
        [SerializeField] Vector3 offset = Vector3.up * 0.4f;
        [SerializeField] float height;
       
        
        public IBarUI CreateBar(Color color, float value,Transform transform, float gravityDirection)
        {
           // Debug.Log("Criando uma barra: "+gravityDirection);
            GameObject o = Instantiate(bar, transform.position, bar.transform.rotation);
            Follow follow = o.GetComponent<Follow>();
            follow.Target = transform;
            offset = GetOffset(yDir: -gravityDirection);
            follow.Offset = offset;
            IBarUI c = o.GetComponent<IBarUI>();
            c.id = Random.Range(0, 1000);
            c.Value = value;
            c.Color = color;
            System.Action<GameObject, bool> handler = null;   
            handler = (go, finished) => 
            {
                RemoveBar(go,gravityDirection);
                c.BarDestroyed -= handler;
            };
            c.BarDestroyed += handler;
            bars.Add(o);
            ReOrderBars(o, gravityDirection);
            return c;
        }
        public void ChangeBarDirection(float gravityDirection)
        {
            offset = GetOffset(yDir: -gravityDirection);
            ReOrderBars(bar, gravityDirection);
        }
        private void RemoveBar(GameObject bar,float gravityDirection)
        {
            bars.Remove(bar);
            ReOrderBars(bar, gravityDirection);
        }
        private Vector3 GetOffset(float xDir = 1,float yDir = 1)
        {
            return new Vector3(Mathf.Abs(offset.x) * xDir, Mathf.Abs(offset.y) * yDir, offset.z);
        }
        private void ReOrderBars(GameObject bar,float gravityDirection)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                bars[i].GetComponent<Follow>().Offset = new Vector3(offset.x, offset.y - (height * i * gravityDirection), offset.z);
            }
        }

        
    }
 }

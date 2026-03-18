using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class Note : MonoBehaviour
    {
        [TextArea(3,100)]
        public string comment;
       
        public Color noteColor;
    }
}

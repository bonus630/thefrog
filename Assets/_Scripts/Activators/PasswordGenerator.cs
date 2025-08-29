using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PasswordGenerator : MonoBehaviour
    {
        [SerializeField] PasswordReceiver passwordReceiver;
        [SerializeField] int fields = 4;
        public List<int> password;
        
        private void Start()
        {
            password = new();
            while (password.Count < fields)
            {
                int random = Random.Range(0, fields);
                if (!password.Contains(random))
                    password.Add(random);
            }
            FindAnyObjectByType<PasswordReceiver>().Password = password;
        }

    }
}

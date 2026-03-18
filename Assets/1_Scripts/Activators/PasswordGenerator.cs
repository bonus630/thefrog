using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PasswordGenerator : IActivator
    {
        [SerializeField] PasswordReceiver passwordReceiver;
        [SerializeField] int fields = 4;
        [field: SerializeField] public int ID { get; set; }
        public List<int> password;
        
        private void Start() => Generate();
        private void Generate()
        {
            password = new();
            while (password.Count < fields)
            {
                int random = Random.Range(0, fields);
                if (!password.Contains(random))
                    password.Add(random);
            }
            if (passwordReceiver == null)
            {
                PasswordReceiver[] ps = FindObjectsByType<PasswordReceiver>(FindObjectsSortMode.InstanceID);
                for (int i = 0; i < ps.Length; i++)
                {
                    if (ps[i].ID == ID)
                        ps[i].Password = password;
                }
            }
            else
                passwordReceiver.Password = password;
        }
        public override void Activate()=> Generate();

        public override void Deactive()
        {
            
        }
    }
}

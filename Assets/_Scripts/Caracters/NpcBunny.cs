using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NpcBunny : NPCBase,INPC
    {
        public void CheckInitialDialogue(int dialogue)
        {
            
        }

        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {
            
        }
    }
}

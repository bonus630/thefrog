using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Manager;

namespace br.com.bonus630.thefrog.Assets.Scripts.Activators
{
    public class ShurykenChestAlert : TipsBase
    {
        private void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.Shuryken))
                Destroy(gameObject);
        }
        protected override void OnEventCompleted(GameEvent obj)
        {
            if(obj.Name.Equals(GameEventName.Shuryken))
                Destroy(gameObject);
        }
    }
}

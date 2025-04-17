using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.com.bonus630.thefrog.Manager;
using Unity.VisualScripting;

namespace br.com.bonus630.thefrog.Activators
{
    public class ShurykenChestAlert : TipsBase
    {
        IEnumerator Start()
        {
            yield return new WaitForNextFrameUnit();
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

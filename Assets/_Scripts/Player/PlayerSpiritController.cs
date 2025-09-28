using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Items;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerSpiritController : PlayerBase
    {
        [SerializeField] List<GameObject> projectilies;
        [SerializeField] GameObject projectileSpawPoint;
        [SerializeField] GameObject projectileSpawPoint2;

        int currentIndex = 0;
        List<ProjectilData> avaliableProjectilies = new();

       // float updateTime = 0.4f;
     //   float time = 0f;
        float count = 0f;
        [field:SerializeField]public ProjectilData CurrentProjectile { get; private set; }

        [SerializeField] Sprite FireSprite;
        [SerializeField] Sprite LightningSprite;
        [SerializeField] Sprite WindSprite;
        [SerializeField] Sprite WaterSprite;
        [SerializeField] Sprite EarthrSprite;
 

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CheckProjectil();
            SetCurrentProjectil();
            GameManager.Instance.eventManager.GameEventCompleted += EventManager_GameEventCompleted;
        }

        private void EventManager_GameEventCompleted(GameEvent obj)
        {
            CheckProjectil();
        }

        // Update is called once per frame
        void Update()
        {
         //   time += Time.deltaTime;
        }

        public void SelectProjectile(float direction)
        {

            //Debug.Log("Direction: " + direction);
          //  if(time > updateTime)
          //  {
            //    time = 0;
                if (direction < 0)
                    currentIndex--;
                if(currentIndex < 0)
                    currentIndex = avaliableProjectilies.Count-1;
                if (direction > 0)
                    currentIndex++;
                if (currentIndex >= avaliableProjectilies.Count)
                    currentIndex = 0;
            Debug.Log("current:" + currentIndex);
                SetCurrentProjectil();
         //       time = 0;
          //  }
        }
        private void CheckProjectil()
        {
            for (int i = 0; i < projectilies.Count; i++)
            {
                var projectile = projectilies[i];
                IProjectilies p = projectile.GetComponent<IProjectilies>();
                if (p == null)
                    return;
                var el = p.GetElement;
                if (avaliableProjectilies.Contains(new ProjectilData(el)))
                    return;
                switch (el)
                {
                    case Elements.Fire:
                        if (GameManager.Instance.IsEventCompleted(GameEventName.FireBall))
                            avaliableProjectilies.Add(new(projectile, projectileSpawPoint, projectileSpawPoint2,Color.red,  Elements.Fire,FireSprite));
                        break;
                    case Elements.Lightining:
                        if (GameManager.Instance.IsEventCompleted(GameEventName.LightningBolt))
                            avaliableProjectilies.Add(new(projectile, projectileSpawPoint,projectileSpawPoint2, Color.white, Elements.Lightining,LightningSprite));
                        break;
                    case Elements.Wind:
                        if (GameManager.Instance.IsEventCompleted(GameEventName.RollingWind))
                            avaliableProjectilies.Add(new(projectile, projectileSpawPoint,projectileSpawPoint2, Color.green,  Elements.Wind,WindSprite));
                        break;
                    case Elements.Water:
                        if (GameManager.Instance.IsEventCompleted(GameEventName.PurifyWater))
                            avaliableProjectilies.Add(new(projectile, projectileSpawPoint, projectileSpawPoint2, Color.blue, Elements.Water,WaterSprite));
                        break;
                    case Elements.Earth:
                        if (GameManager.Instance.IsEventCompleted(GameEventName.None))
                            avaliableProjectilies.Add(new(projectile, projectileSpawPoint,projectileSpawPoint2, Color.magenta,  Elements.Earth, EarthrSprite));
                        break;
                }
            }
            count = avaliableProjectilies.Count;
        }
        private void SetCurrentProjectil()
        {
            if (avaliableProjectilies.Count > 0)
            {
                CurrentProjectile = avaliableProjectilies[currentIndex];
                GameManager.Instance.UpdateProjectil(CurrentProjectile.EffectColor,CurrentProjectile.hud);
            }

        }
       
    }
    [SerializeField]
    public class ProjectilData
    {
        public GameObject Projectil;
        public GameObject SpawnPoint;
        public GameObject SpawnPoint2;
        public Color EffectColor;
        public Elements Element;
        public Sprite hud;

        public ProjectilData(GameObject projectil, GameObject spawnPoint, GameObject spawnPoint2, Color effectColor, Elements element)
        {
            Projectil = projectil;
            SpawnPoint = spawnPoint;
            SpawnPoint2 = spawnPoint2;
            EffectColor = effectColor;
            Element = element;
        }
        public ProjectilData(GameObject projectil, GameObject spawnPoint, GameObject spawnPoint2, Color effectColor, Elements element,Sprite hud)
            :this(projectil,spawnPoint,spawnPoint2,effectColor,element)
        {
            this.hud = hud;
        }
        public ProjectilData(Elements element)
        {
            Element = element;
        }

        // Equals seguro para listas e Contains
        public bool Equals(ProjectilData other)
        {
            if (other == null) return false;
            return this.Element == other.Element;
        }

        // Sobrescrevendo Equals do object
        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectilData);
        }

        // Sempre sobrescreva GetHashCode quando sobrescrever Equals
        public override int GetHashCode()
        {
            return Element.GetHashCode();
        }
    }
}

using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPigPatrol : EnemyToad
    {
        AudioSource m_AudioSource;
        float hooinkTime = 0f;

        protected override void Start()
        {
            base.Start();
            m_AudioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            if(hooinkTime < 0)
            {
                hooinkTime = Random.Range(1.5f, 4f);
            }
            hooinkTime -= Time.deltaTime;
        }
    }
}

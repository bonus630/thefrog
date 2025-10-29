using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace br.com.bonus630.thefrog.Manager
{
    public class CameraBackground : MonoBehaviour
    {
        [SerializeField] DayNightCycleManager cycleManager;
        [SerializeField] GameObject filter;
        [SerializeField] GameObject overlay;
        [SerializeField] GameObject day;
        [SerializeField] GameObject daySunOverlay;
        [SerializeField] GameObject night;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject sunLight;
        [SerializeField] GameObject background2;
        [field: SerializeField] public float CycleDurationMinutes { get; set; } = 1f; 
        [SerializeField][Range(0, 24)] private int hour = 6;

        [SerializeField] Color corNoite = new Color(0.2f, 0.3f, 0.5f, 0.4f);     // Azul escuro, frio
        [SerializeField] Color corAmanhecer = new Color(1f, 0.5f, 0.3f, 0.2f);   // Alaranjado suave
        [SerializeField] Color corMeioDia = new Color(1f, 1f, 0.8f, 0f);         // Luz forte, quase sem filtro
        [SerializeField] Color corAnoitecer = new Color(0.6f, 0.4f, 0.8f, 0.2f); // Roxo suave, frio

        //A camera é 4 de ortho
        //testar em 6.38 no chefe fantasma
        //Criar uma transição de camera


        public int Hour { get { return hour; } }
        float yAmplitude = 20f; // Altura máxima que o sol vai chegar
                      

        SpriteRenderer filterSR;
        public event Action<int> HourChanged;

        bool transitionToNight = false;
        bool transitionToDay = false;
        bool isDay = false;
        bool isNight = false;

        Color transparent = new Color(1f, 1f, 1f, 0f);
        Color white = new Color(1f, 1f, 1f, 1f);
        [SerializeField] Vector3 sunrisePosition; // Posição de início do sol (baixo no horizonte)
        [SerializeField] Vector3 sunsetPosition;  // Posição final do sol (baixo no outro lado)

        Vector3 leftEdge;
        Vector3 rightEdge;

        float sunriseX;
        float sunsetX;
        float sunriseTime = 0.25f; // 6h
        float morningTime = 0.33f; // 8h
        float noonTime = 0.5f; // 12h
        float sunsetTime = 0.75f;  // 18h
        float eveningTime = 0.83f; // 20h
        private void OnValidate()
        {
            if (cycleManager != null)
            {
                InitializeDayByHour(hour);
            }
        }
        void Awake()
        {
            filterSR = filter.GetComponent<SpriteRenderer>();
            cycleManager.cycleDurationMinutes = this.CycleDurationMinutes;
            InitializeDayByHour(this.hour);
            //cycleManager.OnHourChanged += (h) => { this.hour = h;GameManager.Instance.PlayerStates.Hour = h; HourChanged?.Invoke(h); };
            //mudar para eliminar esse aclopamento,espero que funcione ainda
            cycleManager.OnHourChanged += (h) => { this.hour = h; HourChanged?.Invoke(h); };


        }
        private void Start()
        {
            sunriseX = sunrisePosition.x;
            sunsetX = sunsetPosition.x;
        }
        void FixedUpdate()
        {
            float t = cycleManager.cycleTime;

            CheckTransition(t);
            UpdateDayNightSprites(t);
            ApplyFilter(t);
            UpdateSunPosition(t);
            OverlayMovement();
        }
        public void InitializeDayByHour(int hour)
        {
            //Debug.Log("InitializeDayByHour hour:" + hour);
            cycleManager.InitializeByHour(hour);
            this.hour = hour;
        }
        private void UpdateDayNightSprites(float time)
        {
            Color resultColor = Color.white;

            if (isDay && !transitionToDay)
                resultColor = Color.white;
            if (transitionToNight)
            {
                float t = Mathf.InverseLerp(sunsetTime, eveningTime, time);
                resultColor = Color.Lerp(white, transparent, t);
            }
            if (isNight && !transitionToNight)
            {
                resultColor = transparent;
            }
            if (transitionToDay)
            {
                float t = Mathf.InverseLerp(sunriseTime, morningTime, time);
                resultColor = Color.Lerp(transparent, white, t);
            }
            // para noite passamos transparente, para dia passamos branco
            day.GetComponent<SpriteRenderer>().color = resultColor;
            daySunOverlay.GetComponent<SpriteRenderer>().color = resultColor;
            overlay.GetComponent<SpriteRenderer>().color = resultColor;
        }
        private void ApplyFilter(float t)
        {
            Color corAtual;

            if (t < sunriseTime) // 0h - 6h
            {
                corAtual = Color.Lerp(corNoite, corAmanhecer, t / sunriseTime);
            }
            else if (t < noonTime) // 6h - 12h
            {
                corAtual = Color.Lerp(corAmanhecer, corMeioDia, (t - sunriseTime) / sunriseTime);
            }
            else if (t < sunsetTime) // 12h - 18h
            {
                corAtual = Color.Lerp(corMeioDia, corAnoitecer, (t - noonTime) / sunriseTime);
            }
            else // 18h - 24h
            {
                corAtual = Color.Lerp(corAnoitecer, corNoite, (t - sunsetTime) / sunriseTime);
            }

            filterSR.color = corAtual;
        }

        void CheckTransition(float time)
        {
            if (time >= sunriseTime && time <= morningTime)
                transitionToDay = true;
            else if (time >= sunsetTime && time <= eveningTime)
                transitionToNight = true;
            else
            {
                transitionToDay = false;
                transitionToNight = false;
            }
            if (time >= sunriseTime && time < sunsetTime)
            {
                isDay = true;
                isNight = false;
            }
            else
            {
                isDay = false;
                isNight = true;
            }

        }
        public void UpdateSunPosition(float cycleTime)
        {
            // Considerando cycleTime no intervalo [0, 1] -> 0h até 24h
            // Quando cycleTime = 0.25 (6h), queremos X = sunriseX
            // Quando cycleTime = 0.75 (18h), queremos X = sunsetX

            // Define o tempo do nascer e do pôr do sol


            float dayProgress = Mathf.InverseLerp(sunriseTime, sunsetTime, cycleTime); // Progresso no período do dia
            dayProgress = Mathf.Clamp01(dayProgress); // Garante que só movemos o sol entre nascer e pôr

            // Movimento horizontal (interpolação simples entre sunrise e sunset)
            float x = Mathf.Lerp(sunriseX, sunsetX, dayProgress);

            // Movimento vertical (senoide ajustada para começar no horizonte)
            float angle = Mathf.Lerp(0f, Mathf.PI, dayProgress); // 0 radianos ao nascer, PI radianos ao pôr
            float y = Mathf.Sin(angle) * yAmplitude;

            // Atualiza posição
            sun.transform.position = new Vector3(x, y, 0);

            // Exibir ou ocultar o sol
            bool isDay = cycleTime >= sunriseTime && cycleTime <= sunsetTime;
            sun.GetComponent<SpriteRenderer>().enabled = isDay;
            sunLight.GetComponent<Light2D>().enabled = isDay;
            // Atualiza rotação do sol, acho que temos um bug aqui
            Vector3 direction = (Vector3.zero - sunLight.transform.position).normalized;
            float sunAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
            sunLight.transform.rotation = Quaternion.RotateTowards(sunLight.transform.rotation, Quaternion.Euler(0, 0, sunAngle), 500 * Time.fixedDeltaTime);
        }


        void SetAlpha(SpriteRenderer sr, float alpha)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        void OverlayMovement()
        {
            overlay.transform.position += Vector3.left * Time.deltaTime;
            leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
            rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
            if (overlay.transform.position.x + 24 < leftEdge.x)
                overlay.transform.position = new Vector3(rightEdge.x + 24, rightEdge.y + 2, 0);
        }
        public void ChangeBackground()
        {
            background2.SetActive(true);
        }
        public void RestoreBackground()
        {
            background2.SetActive(false);
            
        }
    }
}

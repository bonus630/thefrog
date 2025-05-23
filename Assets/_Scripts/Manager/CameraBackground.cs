﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
namespace br.com.bonus630.thefrog.Manager
{
    public class CameraBackground : MonoBehaviour
    {
        [SerializeField] GameObject filter;
        [SerializeField] GameObject overlay;
        [SerializeField] GameObject day;
        [SerializeField] GameObject daySunOverlay;
        [SerializeField] GameObject night;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject sunLight;
        [SerializeField] GameObject background2;
        [field:SerializeField]public float CycleDurationMinutes { get; set; } = 1f; // Tempo que você quer que dure o ciclo inteiro (12 minutos)
        [SerializeField][Range(0, 24)] private int hour = 6;

        [SerializeField] Color corNoite = new Color(0.2f, 0.3f, 0.5f, 0.4f);     // Azul escuro, frio
        [SerializeField] Color corAmanhecer = new Color(1f, 0.5f, 0.3f, 0.2f);   // Alaranjado suave
        [SerializeField] Color corMeioDia = new Color(1f, 1f, 0.8f, 0f);         // Luz forte, quase sem filtro
        [SerializeField] Color corAnoitecer = new Color(0.6f, 0.4f, 0.8f, 0.2f); // Roxo suave, frio

        //A camera é 4 de ortho
        //testar em 6.38 no chefe fantasma
        //Criar uma transição de camera


        public int Hour { get { return hour; } }

        //float daySeconds = 720f;
        // float currentHour = 0;
        float yAmplitude = 20f; // Altura máxima que o sol vai chegar
        float speed;
        float initialHour;

        SpriteRenderer filterSR;
        public event Action<int> HourChanged;

        bool isToNight = false;
        bool isToDay = false;


        Color transparent = new Color(1f, 1f, 1f, 0f);
        [SerializeField] Vector3 sunrisePosition; // Posição de início do sol (baixo no horizonte)
        [SerializeField] Vector3 sunsetPosition;  // Posição final do sol (baixo no outro lado)

        Vector3 leftEdge;
        Vector3 rightEdge;

        float time;
        void Awake()
        {
            filterSR = filter.GetComponent<SpriteRenderer>();
            speed = 1f / (CycleDurationMinutes * 60f);
            time = CycleDurationMinutes / 6f * 60f; //Duas horas de fad no ciclo, alterar para o 6f para 12 para 1 Hora
            initialHour = InitialTFromHour();

        }
        //private void Start()
        //{
        //    bool isDay = true;
        //    float x = 0;
        //    float y = 0;
        //    int _hour = calculateHour(out x, out y, out isDay);
        //    CheckNight(_hour);
        //}
        void FixedUpdate()
        {
            SunMoviment();
            ApplyFilter();
            OverlayMovement();
        }
        public void InitializeDayByHour(int hour)
        {
            this.hour = hour;

            initialHour = InitialTFromHour();
            if (initialHour > 1)
            {
                day.GetComponent<SpriteRenderer>().color = transparent;
                daySunOverlay.GetComponent<SpriteRenderer>().color = transparent;
                overlay.GetComponent<SpriteRenderer>().color = transparent;
            }
            //fazendo um teste com isso
            CheckNight(hour);
        }

        float InitialTFromHour()
        {
            float t = ((hour - 6f) / 12f);
            if (t < 0f)
                t += 2f;

            return t;
        }
        public void GoToNight(bool now = false)
        {
            if (!isToNight)
            {
                isToNight = true;
                StopAllCoroutines();
                if (now)
                    toNightNow(transparent);
                else
                    StartCoroutine(toNight(Color.white, transparent));
            }
            //day.GetComponent<SpriteRenderer>().color = transparent;
        }
        public void GoToDay(bool now = false)
        {
            if (!isToDay)
            {
                isToDay = true;
                StopAllCoroutines();
                if (now)
                    toNightNow(Color.white);
                else
                    StartCoroutine(toNight(transparent, Color.white));
            }
        }
        IEnumerator toNight(Color s, Color e)
        {
            float currentTime = 0;
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / time);
                day.GetComponent<SpriteRenderer>().color = Color.Lerp(s, e, t);
                daySunOverlay.GetComponent<SpriteRenderer>().color = Color.Lerp(s, e, t);
                overlay.GetComponent<SpriteRenderer>().color = Color.Lerp(s, e, t);
                yield return null;
            }
            toNightNow(e);

        }
        void toNightNow(Color e)
        {
            day.GetComponent<SpriteRenderer>().color = e;
            daySunOverlay.GetComponent<SpriteRenderer>().color = e;
            overlay.GetComponent<SpriteRenderer>().color = e;
            isToNight = false;
            isToDay = false;
        }
        void ApplyFilter()
        {
            Color corAtual;

            if (hour < 6f)
            {
                // Noite profunda
                corAtual = corNoite;
            }
            else if (hour < 12f)
            {
                // Amanhecer -> Meio-dia
                float t = Mathf.InverseLerp(6f, 12f, hour);
                corAtual = Color.Lerp(corAmanhecer, corMeioDia, t);
            }
            else if (hour < 18f)
            {
                // Meio-dia -> Anoitecer
                float t = Mathf.InverseLerp(12f, 18f, hour);
                corAtual = Color.Lerp(corMeioDia, corAnoitecer, t);
            }
            else
            {
                // Anoitecer -> Noite
                float t = Mathf.InverseLerp(18f, 24f, hour);
                corAtual = Color.Lerp(corAnoitecer, corNoite, t);
            }

            // Aplica a cor no filtro (Image)
            filterSR.color = corAtual;
        }
        float sunriseX;
        float sunsetX;
        float amplitude;
        float rotation = 0;
        void SunMoviment()
        {
            bool isDay = true;
            float x = 0;
            float y = 0;
            float angle = 0;
            int _hour = calculateHour(out x, out y, out angle, out isDay);
            if (_hour != hour)
                CheckNight(_hour);
            if (isDay)
            {
                sun.GetComponent<SpriteRenderer>().enabled = true;
                sunLight.GetComponent<Light2D>().enabled = true;
            }
            else
            {
                sun.GetComponent<SpriteRenderer>().enabled = false;
                sunLight.GetComponent<Light2D>().enabled = false;
            }
            // if (sunriseX > 0)
            // {
            //    rotation = 90 * x / sunriseX - 180;
           // Debug.Log("Sun anglex: " + x);
            Vector3 v = (Vector3.zero - sunLight.transform.position).normalized;
            angle = MathF.Atan2(v.y, v.x) * Mathf.Rad2Deg - 180;
           // Debug.Log("Sun angle2: " + angle);
            sunLight.transform.rotation = Quaternion.RotateTowards(sunLight.transform.rotation, Quaternion.Euler(0, 0, angle), 500);
            // }
            sun.transform.position = new Vector3(x, y, 0);
        }
        int calculateHour(out float x, out float y, out float angle, out bool isDay)
        {
            float t = Mathf.Repeat(initialHour + Time.time * speed, 2f);
            x = Mathf.Lerp(sunriseX, sunsetX, t);
            angle = t * Mathf.PI;
            y = Mathf.Sin(angle) * amplitude;
            int _hour = (Mathf.RoundToInt((angle * Mathf.Rad2Deg / 15)) + 6) % 24;
            isDay = t < 1f;

            return _hour;
        }
        void CheckNight(int _hour)
        {
            InvertSun();
            hour = _hour;
            HourChanged?.Invoke(hour);
            GameManager.Instance.PlayerStates.Hour = hour;
           // Debug.Log("Hour: " + hour);
            //  Debug.Log("Tonight: " + isToNight + " ToDay: " + isToDay);

            if (_hour >= 17 && _hour < 19)
                GoToNight();
            if (_hour >= 19 || _hour < 5)
                GoToNight(true);
            if (_hour >= 5 && _hour < 7)
                GoToDay();
            if (_hour >= 7 && _hour < 17)
                GoToDay(true);
        }
        void SunMoviment2()
        {
            InvertSun();
            float cycleDuration = 60f; // Tempo de UM ciclo de dia OU noite (ex: 60s se quiser 1min de dia + 1min de noite)
            float t = Mathf.Repeat(Time.time * speed, cycleDuration * 2f); // Vai de 0 até 2

            bool isDay = t < cycleDuration;

            float progress = isDay ? t / cycleDuration : (2f * cycleDuration - t) / cycleDuration; // 0 → 1 (dia), 1 → 0 (noite)

            // Movimento no eixo X: do nascer ao pôr do sol (ida e volta)
            float x = Mathf.Lerp(sunriseX, sunsetX, progress);

            // Movimento no eixo Y:
            float angle;
            float y;

            if (isDay)
            {
                // Arco alto durante o dia
                angle = progress * Mathf.PI;
                y = Mathf.Sin(angle) * amplitude;
            }
            else
            {
                // Noite: você pode deixar y mais baixo ou negativo, como se sumisse
                angle = progress * Mathf.PI;
                y = -Mathf.Sin(angle) * amplitude * 0.5f; // Um arco mais baixo para noite
            }

            // Hora atual baseada no ciclo de 24 horas
            int _hour = Mathf.RoundToInt(((t / (cycleDuration * 2f)) * 24f)) % 24;

            if (_hour != hour)
            {
                hour = _hour;
                HourChanged?.Invoke(hour);
                //Debug.Log("Hour: " + _hour);
            }

            sun.transform.position = new Vector3(x, y, 0);
        }
        void InvertSun()
        {
            if (hour > 50)
            {
                // GoToNight();
                sunriseX = sunsetPosition.x;
                sunsetX = sunrisePosition.x;
                amplitude = -yAmplitude;
            }
            else
            {
                sunriseX = sunrisePosition.x;
                sunsetX = sunsetPosition.x;
                amplitude = yAmplitude;
            }
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
    }
}

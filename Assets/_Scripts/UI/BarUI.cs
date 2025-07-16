using br.com.bonus630.thefrog.Shared;
using UnityEngine;


namespace br.com.bonus630.thefrog.UI
{
    using UnityEngine;

    using UnityEngine;

    public class BarUI : MonoBehaviour, IBarUI
    {
        [SerializeField] GameObject root;
        [SerializeField] GameObject barSprite;

        [field: SerializeField]
        public Color Color
        {
            get => barSprite.GetComponent<SpriteRenderer>().color;
            set => barSprite.GetComponent<SpriteRenderer>().color = value;
        }

        [SerializeField] Transform bar;

        private float currentValue = 0;       // valor exibido
        private float minValue = 0;
        private float maxValue = 100;
        private float targetValue = 0;        // destino desejado
        private float startValue = 0;

        private float timeToUpdateTotal = 0f;
        private float elapsedTime = 0f;
        private bool isAnimating = false;

        [field: SerializeField]
        public float MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                UpdateScale();
            }
        }

        [field: SerializeField]
        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                UpdateScale();
            }
        }

        [field: SerializeField]
        public float Value
        {
            get => targetValue;
            set
            {
                // aplica imediatamente, sem animar
                targetValue = Mathf.Clamp(value, minValue, maxValue);
                currentValue = targetValue;
                isAnimating = false;
                UpdateScale();
            }
        }

        //private void Start()
        //{
        //    currentValue = targetValue = minValue;
        //    UpdateScale();
        //}

        private void Update()
        {
            if (!isAnimating)
                return;

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / timeToUpdateTotal);
            currentValue = Mathf.Lerp(startValue, targetValue, t);
            UpdateScale();

            if (t >= 1f)
            {
                isAnimating = false;
            }
        }

        private float GetNormalizedValue(float value)
        {
            if (Mathf.Approximately(maxValue, minValue)) return 0;
            return Mathf.InverseLerp(minValue, maxValue, value);
        }

        private void UpdateScale()
        {
            float normalized = GetNormalizedValue(currentValue);
            bar.localScale = new Vector3(normalized, 1, 1);
        }

        public void GoToValue(float value, float time)
        {
            value = Mathf.Clamp(value, minValue, maxValue);

            if (Mathf.Approximately(currentValue, value) || time <= 0)
            {
                // atualiza imediatamente se já está no destino ou tempo for zero
                Value = value;
                return;
            }

            startValue = currentValue;
            targetValue = value;
            elapsedTime = 0f;
            timeToUpdateTotal = time;
            isAnimating = true;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }


}

using br.com.bonus630.thefrog.Shared;
using UnityEngine;


namespace br.com.bonus630.thefrog.UI
{
    public class BarUI : MonoBehaviour, IBarUI
    {
        [SerializeField] GameObject root;
        [SerializeField] GameObject barSprite;

        [field: SerializeField] public Color Color { get { return barSprite.GetComponent<SpriteRenderer>().color; } set { barSprite.GetComponent<SpriteRenderer>().color = value; } }
        [SerializeField] Transform bar;

        [SerializeField] float increment = 1;
        private float value = 0;
        private float minValue = 0;
        private float maxValue = 100;
        [field: SerializeField] public float Value { get { return this.value; } set { this.value = value; setValue(value); } }
        [field: SerializeField] public float MinValue { get { return this.value; } set { this.value = value; calcIncrement(); } }
        [field: SerializeField] public float MaxValue { get { return this.value; } set { this.value = value; calcIncrement(); } }

        float tempValue;
        float timeToUpdate = 0f;
        float timer = 0f;


        void setValue(float value)
        {
            // Debug.Log("SetValue: " + value);
            tempValue = value;
            bar.localScale = new Vector3(GetValue(tempValue), 1, 1);
        }
        void calcIncrement()
        {
            this.increment = (maxValue - minValue) / 100;
        }
        void Update()
        {
            bar.localScale = new Vector3(GetValue(tempValue), 1, 1);
            if (timeToUpdate == 0)
            {
                tempValue = this.value;
                return;
            }
            // Debug.Log("ScaleX: "+bar.localScale.x);
            if (timer > timeToUpdate)
            {
                if (tempValue > (this.value))
                {
                    tempValue -= increment;
                }
                if (tempValue < (this.value))
                {
                    tempValue += increment;
                }
                timer = 0f;
            }
            else
                timer += Time.deltaTime;

                    
        }

        private float GetValue(float value) => value / (MaxValue - MinValue);

        public void GoToValue(float value, float time)
        {
            this.value = value;

            float diff = Mathf.Abs(value - tempValue);
            if (diff != 0)
                timeToUpdate = time / diff;
            // Debug.Log("timetoupadar :"+timeToUpdate);
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }

    }
}

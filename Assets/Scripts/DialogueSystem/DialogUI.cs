using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    public class DialogUI : MonoBehaviour
    {

        [SerializeField] Image background;
        [SerializeField] Image avatar;
        //[SerializeField] Sprite sprite;
        [SerializeField] TextMeshProUGUI text;
        //TextMeshProUGUI name;
        public float speed = 0.1f;
        public bool open = false;

        Color white = Color.white;
        Color transparent = Color.white;

        private void Awake()
        {
            white.a = 255;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            transparent.a = 0;
            transparent.r = 255;
            transparent.g = 255;
            transparent.b = 255;
            //background = transform.GetChild(0).GetComponent<Image>();
            //name = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            //text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (open)
            {
                background.color = Color.Lerp(white, transparent, speed * Time.deltaTime);
            }
            else
            {


                background.color = Color.Lerp(transparent, white, speed * Time.deltaTime);
            }

        }
        public void SetAvatar(Sprite avatar)
        {
            Image i = this.avatar.GetComponent<Image>();
            i.sprite = avatar;
            // avatar.GetComponent sprite = avatar;
        }
        public void SetName(string Name)
        {
            // name.text = Name;
        }
        public void Enable()
        {
            //background.fillAmount = 0;
            open = true;
            background.gameObject.SetActive(true);
            avatar.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            // background.color = white;

        }
        public void Disable()
        {
            open = false;
            text.text = string.Empty;
            background.gameObject.SetActive(false);
            avatar.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            //  background.color = transparent;
            //name.text = string.Empty;
        }
    }
}

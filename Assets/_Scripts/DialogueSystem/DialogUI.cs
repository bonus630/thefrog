using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    public class DialogUI : MonoBehaviour
    {

        [SerializeField] Image background;
        [SerializeField] Image avatar;
        [SerializeField] Image haveMoreIcon;
        //[SerializeField] Sprite sprite;
        [SerializeField] TextMeshProUGUI text;
        //TextMeshProUGUI name;
        public float speed = 0.1f;
        public bool open = false;

        Color white = Color.white;
        Color transparent = Color.white;
        [SerializeField] float topPosition = 560f;
        [SerializeField] float bottomPosition = 90f;
        public DialogPosition CurrentPosition { get; private set; } = DialogPosition.Bottom;

        public RectTransform Rect { get; set; }
        private RectTransform avatarRect;
        public float avatarOffset = 50f;  // espaço entre avatar e caixa

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
            Rect = GetComponent<RectTransform>();
            avatarRect = avatar.GetComponent<RectTransform>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (open)
            //{
            //    background.color = Color.Lerp(white, transparent, speed * Time.deltaTime);
            //}
            //else
            //{
            //    background.color = Color.Lerp(transparent, white, speed * Time.deltaTime);
            //}

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
            SetHaveMoreIcon(true);
            open = true;
           // background.gameObject.SetActive(true);
            avatar.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            background.color = white;

        }
        public void Disable()
        {
            open = false;
            text.text = string.Empty;
           // background.gameObject.SetActive(false);
            avatar.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            SetHaveMoreIcon(false);
            background.color = transparent;
            SetPosition(DialogPosition.Bottom);
            //name.text = string.Empty;
        }
        //meu codigo
        public void SetPosition(DialogPosition position)
        {
            Debug.Log("Position :" + position);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, position == DialogPosition.Top ? topPosition : bottomPosition);
            if (position != CurrentPosition)
            {
                Debug.Log("Avatar " + GetComponent<RectTransform>().anchoredPosition);
                avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(avatar.GetComponent<RectTransform>().anchoredPosition.x, avatar.GetComponent<RectTransform>().anchoredPosition.y * -1);
                CurrentPosition = position;
            }
        }
        //chatgpt codigo
        //public void SetPosition(DialogPosition position)
        //{
        //    // Caixa sobe ou desce
        //    float y = (position == DialogPosition.Top) ? topPosition : bottomPosition;
        //    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
        //    Debug.Log("Dialog y:" + y);
        //    // Avatar sempre acompanha a caixa
        //    if (position == DialogPosition.Top)
        //    {
        //        // Player embaixo -> Caixa em cima -> Avatar abaixo da caixa
        //        avatarRect.anchoredPosition = new Vector2(
        //            avatarRect.anchoredPosition.x,
        //            -Mathf.Abs(avatarOffset)
        //        );
        //    }
        //    else
        //    {
        //        // Player em cima -> Caixa embaixo -> Avatar acima da caixa
        //        avatarRect.anchoredPosition = new Vector2(
        //            avatarRect.anchoredPosition.x,
        //            Mathf.Abs(avatarOffset)
        //        );
        //    }

        //    CurrentPosition = position;
        //}
        public void SetHaveMoreIcon(bool value)
        {
            haveMoreIcon.gameObject.SetActive(value);
        }
    }
    public enum DialogPosition 
    {
        Top, 
        Bottom
    }
}

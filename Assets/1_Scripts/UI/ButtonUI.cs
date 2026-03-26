using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace br.com.bonus630.thefrog.UI
{
    public class ButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
    {
        [SerializeField] GameObject Image;
        [SerializeField] AudioSource au;
        private Button btn;
        // bool isPlayed = false;

        private void Awake()
        {
            btn = GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(OnClick);
            
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Image.SetActive(false);
        }
        private void OnDisable()
        {
            Image.SetActive(false);
        }
        public void OnSelect(BaseEventData eventData)
        {
            Image.SetActive(true);
            au.Play();
        }
        private void OnClick()
        {
            Image.SetActive(false);
        }
        //private void Update()
        //{
        //    bool isSelected = EventSystem.current.currentSelectedGameObject == gameObject;
        //    Image.SetActive(isSelected);
        //    if (isSelected && !isPlayed)
        //    {
        //        au.Play();
        //    }
        //    else if (!isSelected && isPlayed)
        //    {
        //        isPlayed = false;
        //    }
        //}
    }
}

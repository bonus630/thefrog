using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace br.com.bonus630.thefrog.UI
{
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField] GameObject Image;
        [SerializeField] AudioSource au;
        bool isPlayed = false;

        private void Update()
        {
            bool isSelected = EventSystem.current.currentSelectedGameObject == gameObject;
            Image.SetActive(isSelected);
            if (isSelected && !isPlayed)
            {
                au.Play();
            }
            else if (!isSelected && isPlayed)
            {
                isPlayed = false;
            }


        }
    }
}

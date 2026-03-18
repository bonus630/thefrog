using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.UI
{
    public class Controls : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] GameObject xboxControlImage;
        [SerializeField] GameObject psControlImage;
        [SerializeField] GameObject keyboardControlImage;


        private void OnEnable()
        {
            if (playerInput.currentControlScheme.Equals("Gamepad"))
            {
                xboxControlImage.SetActive(true);
                keyboardControlImage.SetActive(false);
            }
            else
            {
                xboxControlImage.SetActive(false);
                keyboardControlImage.SetActive(true);
            }
        }

    }
}

using br.com.bonus630.thefrog.Utils;
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
            StartCoroutine(CoroutineUtil.WaitUntilThen(CheckControlScheme, CheckCurrentControl));
        }

        private void CheckControlScheme()
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
        private bool CheckCurrentControl() => playerInput.currentControlScheme != null;
    }
}

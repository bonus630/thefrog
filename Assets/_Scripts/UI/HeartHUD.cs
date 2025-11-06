using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.UI
{
    public class HeartHUD : MonoBehaviour
    {
        //vamos começar depois refatoramos
        int maxColHearts = 10;
        public void UpdateHeart(int hearts)
        {
            GameObject hud = transform.GetChild(0).gameObject;
            //GetPlayerScript.CurrentLife += hearts;
          //  this.PlayerStates.Hearts += hearts;

            if (hearts > 0)
            {
                StartCoroutine(AddHeart(hud, hearts));
            }

            if (hearts < 0)
            {
                StartCoroutine(RemoveHeart(hud, hearts));
            }
        }
        public void UpdateMaxHearts(int hearts)
        {
            //this.PlayerStates.MaxHearts += hearts;
            UpdateHeart(hearts);
        }
        private void UpdateHearts(int hearts)
        {
            GameObject hud = transform.GetChild(0).gameObject;
            StartCoroutine(AddHeart(hud, hearts - 1));
        }
        IEnumerator AddHeart(GameObject hud, int hearts)
        {
            int heartCount = hud.transform.childCount;
            int total = hearts + heartCount;
            GameObject heart = hud.transform.GetChild(0).gameObject;
            GameObject lastHeart = hud.transform.GetChild(heartCount - 1).gameObject;
            var rect = hud.GetComponent<RectTransform>();
            var heartRect = heart.GetComponent<RectTransform>();
            int col = heartCount % maxColHearts;
            int row = heartCount / maxColHearts;

            while (total > hud.transform.childCount)
            {
                var gb = Instantiate(heart, rect, false);
                //Debug.Log("Col: " + col + " Row: " + row);
                float offsetX = (heartRect.sizeDelta.x + 0.5f) * col;
                float offsetY = (-heartRect.sizeDelta.y - 0.5f) * row;
                gb.GetComponent<RectTransform>().anchoredPosition = gb.GetComponent<RectTransform>().anchoredPosition + new Vector2(offsetX, offsetY);
                col++;
                if (col >= maxColHearts)
                {
                    row++;
                    col = 0;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator RemoveHeart(GameObject hud, int hearts)
        {
            int toRemove = hearts;
            while (toRemove < 0)
            {
                Destroy(hud.transform.GetChild(hud.transform.childCount - 1).gameObject);
                toRemove++;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}

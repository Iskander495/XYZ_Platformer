using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Interactions
{
    public class FadeComponent : MonoBehaviour
    {
        /// <summary>
        /// Скрываем объект и всех детей
        /// </summary>
        public void FadeOut()
        {
            foreach (var child in getChildrens())
            {
                StartCoroutine(FadeObject(child, true));
            }

            StartCoroutine(FadeObject(gameObject, true));
        }

        /// <summary>
        /// Показываем объект и всех детей
        /// </summary>
        public void FadeIn()
        {
            foreach (var child in getChildrens())
            {
                StartCoroutine(FadeObject(child, false));
            }

            StartCoroutine(FadeObject(gameObject, false));
        }

        private GameObject[] getChildrens()
        {
            List<GameObject> childs = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var tr = transform.GetChild(i);

                childs.Add(tr.gameObject);
            }

            return childs.ToArray();
        }


        IEnumerator FadeObject(GameObject target, bool fadeAway)
        {
            var sprite = target.GetComponent<SpriteRenderer>();

            if(sprite == null) 
                yield return null;

            if (fadeAway) // скрываем
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime + 0.15f)
                {
                    sprite.color = new Color(1, 1, 1, i);
                    yield return null;
                }
            }
            else // показываем
            {
                // loop over 1 second
                for (float i = 0; i <= 1; i += Time.deltaTime + 0.15f)
                {
                    // set color with i as alpha
                    sprite.color = new Color(1, 1, 1, i);
                    yield return null;
                }
            }
        }
    }
}
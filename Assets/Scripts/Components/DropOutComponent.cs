using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Components {

    /// <summary>
    /// Реализация вываливающегося стаффа (например из бочек)
    /// </summary>
    public class DropOutComponent : MonoBehaviour
    {
        /// <summary>
        /// Общее количество выпадающего шмурдяка
        /// </summary>
        [SerializeField] private int _count;

        /// <summary>
        /// Сам шмурдяк
        /// </summary>
        [SerializeField] private List<DropItem> _items;

        /// <summary>
        /// Вываливаем что есть
        /// </summary>
        public void DropOut()
        {
            List<DropItem> toCreate = new List<DropItem>();

            int i = 0;
            while(i < _count)
            {
                var rand = UnityEngine.Random.Range(0, 100);

                foreach(var itm in _items)
                {
                    if (itm.PlayOut(rand))
                    {
                        toCreate.Add(itm);
                        i++;
                    }
                }
            }

            Vector3 newPosition = transform.position;
            foreach (var obj in toCreate)
            {
                var itm = Instantiate(obj.item, transform.position, Quaternion.identity);

                var objHeight = itm.GetComponent<SpriteRenderer>()?.bounds.size.y;
                
                newPosition.y += (float)objHeight;

                itm.transform.position = newPosition;

                /*
                var rb = itm.GetComponent<Rigidbody2D>();
                if(!rb)
                {
                    rb = itm.AddComponent<Rigidbody2D>();
                }

                rb.mass = 0.001f;
                var collider = itm.AddComponent<BoxCollider2D>();
                rb.velocity = new Vector2(UnityEngine.Random.Range(-0.001f, 0.001f), 2f);
                */
            }
        }
    }

    [Serializable]
    public class DropItem
    {
        [SerializeField] [Range(0, 100)] private int _chance;

        [SerializeField] public GameObject item;

        private List<int> _numbers = new List<int>();

        /// <summary>
        /// Определение, попало ли указанное число в те числа, что есть у объекта
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool PlayOut(int number)
        {
            fillItemNumbers();

            if (_numbers.Contains(number))
                return true;

            return false;
        }

        /// <summary>
        /// Генерируем числа, количеством равным проценту шанса выпадения
        /// </summary>
        public void fillItemNumbers()
        {
            while(_numbers.Count < _chance)
            {
                var rand = UnityEngine.Random.Range(0, 100);
                
                if(!_numbers.Contains(rand))
                    _numbers.Add(rand);
            }
        }
    }
}
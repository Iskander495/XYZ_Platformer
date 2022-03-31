using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components {
    public class DropOutCoinsComponent : MonoBehaviour
    {
        /// <summary>
        /// Сколько вывалится денег
        /// </summary>
        [SerializeField] private int _dropCoinsCount;

        /// <summary>
        /// Префаб монетки
        /// </summary>
        [SerializeField] private GameObject _prefab;

        /// <summary>
        /// Размер ускорения по вертикали
        /// </summary>
        [SerializeField] [Range(0, 20)] private float _yVelocity;

        /// <summary>
        /// Минимальное ускорение по горизонтали
        /// </summary>
        [SerializeField] [Range(0, -10)] private float _xVelocityMin;

        /// <summary>
        /// Максимальное ускорение по горизонтали
        /// </summary>
        [SerializeField] [Range(0, 10)] private float _xVelocityMax;

        public void DropOut(GameObject other)
        {
            var coinsComponent = gameObject.GetComponent<AddCoinsComponent>();

            var dropCount = _dropCoinsCount;
            if (_dropCoinsCount > coinsComponent.Count())
                dropCount = coinsComponent.Count();

            StartCoroutine(Drop(coinsComponent, dropCount, other.transform));
        }

        private IEnumerator Drop(AddCoinsComponent coinsComponent, int count, Transform target)
        {
            for (int i = 0; i < count; i++)
            {
                var newObj = Instantiate(_prefab, target.position, Quaternion.identity);
                //newObj.transform.localScale = gameObject.transform.lossyScale;

                var rb = newObj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(UnityEngine.Random.Range(_xVelocityMin, _xVelocityMax), _yVelocity);

                coinsComponent.DecreaseCoins(1);

                yield return null;
            }
        }
    }
}
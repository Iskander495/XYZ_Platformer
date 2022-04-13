using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components {
    public class DropOutCoinsComponent : MonoBehaviour
    {
        /// <summary>
        /// Сколько вывалится денег
        /// </summary>
        [SerializeField] private int _dropCoinsCount = 5;

        /// <summary>
        /// Префаб монетки
        /// </summary>
        [SerializeField] private GameObject _prefab;

        /// <summary>
        /// Угол разброса префабов
        /// </summary>
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private float _speed = 6;


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
            for (int i = 0; i < _dropCoinsCount; i++)
            {
                var newObj = Instantiate(_prefab, target.position, Quaternion.identity);

                var rigidBody = newObj.GetComponent<Rigidbody2D>();

                var randomAngle = Random.Range(0, _sectorAngle);
                var forceVector = AngleToVectorInSector(randomAngle);

                rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);

                coinsComponent.DecreaseCoins(1);

                yield return null;
            }
        }

        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }
    }
}
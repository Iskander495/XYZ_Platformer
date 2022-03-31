using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components
{
    public class TeleportComponent : MonoBehaviour
    {
        /// <summary>
        /// Точка назначения
        /// </summary>
        [SerializeField] private Transform _destinationPosition;
        /// <summary>
        /// Время "исчезновения"
        /// </summary>
        [SerializeField] private float _alphaTime = 0.8f;
        /// <summary>
        /// Время перемещения
        /// </summary>
        [SerializeField] private float _moveTime = 0.8f;

        public void Teleport(GameObject _targetObject)
        {
            //_targetObject.transform.position = _destinationPosition.position;

            StartCoroutine(AnimateTeleport(_targetObject));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var input = target.GetComponent<PlayerInput>();

            // отключаем управление героем
            SetLockInput(input, true);

            // исчезаем
            yield return AlphaAnimation(sprite, 0);

            // "выключаем" героя, чтоб не взаимодействовать с миром, на время переноса
            target.SetActive(false);

            // перемещаемся
            yield return MoveAnimation(target);

            // включаем героя
            target.SetActive(true);

            // появляемся
            yield return AlphaAnimation(sprite, 1);

            // включаем управление героем
            SetLockInput(input, false);
        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;

                target.transform.position = Vector3.Lerp(target.transform.position, _destinationPosition.transform.position, moveTime / _moveTime);

                yield return null;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var alphaTime = 0f;
            var defaultAlfa = sprite.color.a;

            while (alphaTime < _alphaTime)
            {
                alphaTime += Time.deltaTime;

                var tmpAlpha = Mathf.Lerp(defaultAlfa, destAlpha, alphaTime / _alphaTime);

                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                // пропускаем кадр
                yield return null;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimaion : MonoBehaviour
    {
        /// <summary>
        /// Признак зацикленности анимации
        /// </summary>
        [SerializeField] private bool _loop;

        /// <summary>
        /// Кадров в секунду
        /// </summary>
        [SerializeField] private int _frameRate;

        /// <summary>
        /// Массив спрайтов
        /// </summary>
        [SerializeField] private Sprite[] _sprites;

        /// <summary>
        /// Event, вызывающийся после окончания анимации
        /// </summary>
        [SerializeField] private UnityEvent _onComlpeteEvent;

        /// <summary>
        /// Текущий спрайт
        /// </summary>
        private int _currentSpriteIndex;

        /// <summary>
        /// Время, когда должны обновить спрайт
        /// </summary>
        private float _nextFrameTime;

        /// <summary>
        /// Количество кадров в секунду
        /// </summary>
        private float _secondsPerFrame;

        private bool _isPlaying = true;

        private bool _onFrame = false;

        ////// COMPONENTS ///////
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
        }

        private void Update()
        {
            // пропускаем кадр
            if (!_onFrame || !_isPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _isPlaying = false;
                    _onComlpeteEvent?.Invoke();
                    return;
                }
            }

            // сменяем кадр
            _renderer.sprite = _sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        /// <summary>
        /// Вошли в кадр
        /// </summary>
        private void OnBecameVisible()
        {
            _onFrame = true;
        }

        /// <summary>
        /// Вышли из кадра
        /// </summary>
        private void OnBecameInvisible()
        {
            _onFrame = false;
        }
    }
}
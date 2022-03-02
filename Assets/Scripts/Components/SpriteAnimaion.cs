using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimaion : MonoBehaviour
    {
        [SerializeField] private SpriteAnimationItem[] _animations;

        [SerializeField] private SpriteAnimationItem _currentSpriteItem;



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

            if (_animations.Length > 0)
                SetClip(_animations[0]._name);
        }

        private void Update()
        {
            // пропускаем кадр
            if (!_onFrame || !_isPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _currentSpriteItem._sprites.Length)
            {
                if (_currentSpriteItem._loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _isPlaying = false;
                    _currentSpriteItem._onComlpeteEvent?.Invoke();

                    if (_currentSpriteItem._allowNext) {
                        var newIdx = GetAnimationIndexByName(_currentSpriteItem._name) + 1;

                        if (newIdx >= 0)
                            _currentSpriteItem = _animations[newIdx];
                    }

                    return;
                }
            }

            // сменяем кадр
            _renderer.sprite = _currentSpriteItem._sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetClip(string name)
        {
            foreach(SpriteAnimationItem item in _animations)
            {
                if(item._name.Equals(name))
                {
                    _currentSpriteItem = item;

                    _secondsPerFrame = 1f / _currentSpriteItem._frameRate;
                    _nextFrameTime = Time.time + _secondsPerFrame;
                    _currentSpriteIndex = 0;

                    return;
                }
            }

            throw new System.Exception($"SpriteAnimationItem {name} not found");
        }

        private int GetAnimationIndexByName(string name)
        {
            int idx = -1;
            foreach (SpriteAnimationItem item in _animations)
            {
                idx++;
                if (item._name.Equals(name))
                    return idx;
                    
            }

            return -1;
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

    [Serializable]
    public class SpriteAnimationItem
    {
        /// <summary>
        /// имя стэйта
        /// </summary>
        [SerializeField] public string _name;

        /// <summary>
        /// может ли стэйт переключиться на следующий по окончанию спрайтов
        /// </summary>
        [SerializeField] public bool _allowNext;

        /// <summary>
        /// Признак зацикленности анимации
        /// </summary>
        [SerializeField] public bool _loop;

        /// <summary>
        /// Кадров в секунду
        /// </summary>
        [SerializeField] public int _frameRate;

        /// <summary>
        /// Массив спрайтов
        /// </summary>
        [SerializeField] public Sprite[] _sprites;

        /// <summary>
        /// Event, вызывающийся после окончания анимации
        /// </summary>
        [SerializeField] public UnityEvent _onComlpeteEvent;
    }
}
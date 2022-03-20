using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimaion : MonoBehaviour
    {
        /// <summary>
        /// Кадров в секунду
        /// </summary>
        [SerializeField] [Range(1, 30)] private int _frameRate;

        [SerializeField] private AnimationClip[] _animations;

        private AnimationClip _currentClip;

        /// <summary>
        /// Event, вызывающийся после окончания анимации
        /// </summary>
        [SerializeField] public UnityEvent<string> onComlpete;

        /// <summary>
        /// Текущий спрайт
        /// </summary>
        private int _currentFrame;

        /// <summary>
        /// Время, когда должны обновить спрайт
        /// </summary>
        private float _nextFrameTime;

        /// <summary>
        /// Количество кадров в секунду
        /// </summary>
        private float _secondsPerFrame;

        private bool _isPlaying = true;

        ////// COMPONENTS ///////
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            _secondsPerFrame = 1f / _frameRate;

            if (_animations.Length > 0)
                SetClip(_animations[0].name);
        }

        private void Update()
        {
            // пропускаем кадр
            if (_nextFrameTime > Time.time) return;

            if (_currentFrame >= _currentClip.sprites.Length)
            {
                if (_currentClip.loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    enabled = _isPlaying = _currentClip.allowNextClip;

                    _currentClip.onComlpeteEvent?.Invoke();
                    onComlpete?.Invoke(_currentClip.name);

                    if (_currentClip.allowNextClip) {
                        _currentFrame = 0;

                        var newIdx = GetAnimationIndexByName(_currentClip.name) + 1;

                        if (newIdx >= 0)
                            _currentClip = _animations[newIdx];
                    }

                    return;
                }
            }

            // сменяем кадр
            _renderer.sprite = _currentClip.sprites[_currentFrame];
            _nextFrameTime += _secondsPerFrame;
            _currentFrame++;
        }

        public void SetClip(string name)
        {
            foreach(AnimationClip item in _animations)
            {
                if(item.name.Equals(name))
                {
                    _currentClip = item;
                    StartAnimation();
                    return;
                }
            }


            enabled = _isPlaying = false;

            throw new System.Exception($"SpriteAnimationItem {name} not found");
        }

        public void StartAnimation()
        {
            enabled = _isPlaying = true;

            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentFrame = 0;

        }

        public void OnEnable()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
        }

        public void SetClip(int index)
        {
            if (_animations.Length > index) {
                SetClip(_animations[index].name);
            }
        }

        private int GetAnimationIndexByName(string name)
        {
            int idx = -1;
            foreach (AnimationClip item in _animations)
            {
                idx++;
                if (item.name.Equals(name))
                    return idx;
                    
            }

            return -1;
        }

        /// <summary>
        /// Вошли в кадр
        /// </summary>
        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        /// <summary>
        /// Вышли из кадра
        /// </summary>
        private void OnBecameInvisible()
        {
            enabled = false;
        }
    }

    [Serializable]
    public class AnimationClip
    {
        /// <summary>
        /// имя стэйта
        /// </summary>
        [SerializeField] public string name;

        /// <summary>
        /// может ли стэйт переключиться на следующий по окончанию спрайтов
        /// </summary>
        [SerializeField] public bool allowNextClip;

        /// <summary>
        /// Признак зацикленности анимации
        /// </summary>
        [SerializeField] public bool loop;

        /// <summary>
        /// Массив спрайтов
        /// </summary>
        [SerializeField] public Sprite[] sprites;

        /// <summary>
        /// Event, вызывающийся после окончания анимации
        /// </summary>
        [SerializeField] public UnityEvent onComlpeteEvent;

    }
}
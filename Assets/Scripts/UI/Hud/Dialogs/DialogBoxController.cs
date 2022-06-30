using Model.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Components.UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space]
        [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;

        private Coroutine _typingRoutine;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentSentence = 0;
            _text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);

            _animator.SetBool(IsOpen, true);
        }

        public void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;

            var sentence = _data.Sentences[_currentSentence];
            foreach(var letter in sentence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();

            _text.text = _data.Sentences[_currentSentence];
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentence++;

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
            if(isDialogCompleted)
            {
                HideDialogBox();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            
            _typingRoutine = null;
        }

        public void OnCloseAnimationComplete()
        {

        }

        [SerializeField] private DialogData _testData;
        public void Test()
        {
            ShowDialog(_testData);
        }
    }
}
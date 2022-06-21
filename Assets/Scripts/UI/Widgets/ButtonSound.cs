using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Components.UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _clip;

        private AudioSource _source;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_source == null)
                _source = AudioUtils.FindSfxSource();

            _source.PlayOneShot(_clip);
        }
    }
}
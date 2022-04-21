using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioData[] _sounds;


        public void Play(string id)
        {
            foreach(var sound in _sounds)
            {
                if (!sound.Id.Equals(id)) continue;

                _source.PlayOneShot(sound.Clip);
                break;
            }
        }
    }

    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}
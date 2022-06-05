using Components.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlaySfxSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private AudioSource _source;

    public void Play()
    {
        if (_source == null)
            _source = AudioUtils.FindAudioSource();

        _source.PlayOneShot(_clip);
    }
}

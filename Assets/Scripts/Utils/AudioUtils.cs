using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class AudioUtils : MonoBehaviour
    {
        public static string SfxSourceTag = "SfxAudioSource";

        public static AudioSource FindSfxSource()
        {
            return GameObject.FindWithTag(SfxSourceTag)?.GetComponent<AudioSource>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SetAudioMixer : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private List<AudioMixerGroup> _ignoreMixers = new List<AudioMixerGroup>();
    private Dictionary<AudioSource, AudioMixerGroup> _itemsOnArea = new Dictionary<AudioSource, AudioMixerGroup>();

    public void SetMixer(GameObject go)
    {
        var sources = go.GetComponentsInChildren<AudioSource>().ToList();
        var parent = go.GetComponent<AudioSource>();
        if (parent != null && !sources.Contains(parent)) 
            sources.Add(parent);

        if (sources.Count <= 0) return;

        foreach (var audioSource in sources)
        {
            if (_ignoreMixers.Contains(audioSource.outputAudioMixerGroup)) continue;

            _itemsOnArea.Add(audioSource, audioSource.outputAudioMixerGroup);
            audioSource.outputAudioMixerGroup = _mixer;
        }
    }

    public void ResetMixer(GameObject go)
    {
        var sources = go.GetComponentsInChildren<AudioSource>().ToList();
        var parent = go.GetComponent<AudioSource>();
        if (parent != null && !sources.Contains(parent))
            sources.Add(parent);

        if (sources.Count <= 0) return;

        foreach (var audioSource in sources)
        {
            if (_itemsOnArea.ContainsKey(audioSource))
            {
                audioSource.outputAudioMixerGroup = _itemsOnArea[audioSource];
                _itemsOnArea.Remove(audioSource);
            }
        }
    }
}

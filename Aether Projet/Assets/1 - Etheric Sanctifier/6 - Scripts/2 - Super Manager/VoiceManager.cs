using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VoiceType
{
    Intro,
    Tuto01,
    Tuto02,
    Tuto03
}

public class VoiceManager : MonoBehaviour
{
    [Header("Prefab Voice")]
    [SerializeField] private GameObject _voicePrefab3D;

    [Header("Voices")]
    [SerializeField] private AudioClip _voiceIntro;
    [SerializeField] private AudioClip _voiceTuto01;
    [SerializeField] private AudioClip _voiceTuto02;
    [SerializeField] private AudioClip _voiceTuto03;

    private GameObject InstantiateVoice()
    {
        return Instantiate(_voicePrefab3D);
    }

    public void PlayVoice(VoiceType voicetype, float volume)
    {
        GameObject voice = InstantiateVoice();
        AudioSource voiceSource = voice.GetComponent<AudioSource>();

        switch (voicetype)
        {
            case VoiceType.Intro:
                voiceSource.clip = _voiceIntro;
                break;

            case VoiceType.Tuto01:
                voiceSource.clip = _voiceTuto01;
                break;

            case VoiceType.Tuto02:
                voiceSource.clip = _voiceTuto02;
                volume = 0.2f;
                break;

            case VoiceType.Tuto03:
                voiceSource.clip = _voiceTuto03;
                break;
        }

        voiceSource.volume = volume;
        voiceSource.Play();
    }
}

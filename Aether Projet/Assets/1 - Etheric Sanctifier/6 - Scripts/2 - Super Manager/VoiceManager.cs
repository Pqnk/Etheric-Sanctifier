using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VoiceType
{
    Intro,
    Tuto01,
    Tuto02,
    Tuto03,
    Tuto04,
    HUB01,
    HUB02,
    HUB03,
    HUB04,
    HUB05
}

public class VoiceManager : MonoBehaviour
{
    [Header("Prefab Voice")]
    [SerializeField] private GameObject _voicePrefab3D;

    [Header("Voices HUB")]
    [SerializeField] private AudioClip _voiceIntro;
    [SerializeField] private AudioClip _voiceHUB01;
    [SerializeField] private AudioClip _voiceHUB02;
    [SerializeField] private AudioClip _voiceHUB03;
    [SerializeField] private AudioClip _voiceHUB04;
    [SerializeField] private AudioClip _voiceHUB05;

    [Header("Voices Tutorial")]
    [SerializeField] private AudioClip _voiceTuto01;
    [SerializeField] private AudioClip _voiceTuto02;
    [SerializeField] private AudioClip _voiceTuto03;
    [SerializeField] private AudioClip _voiceTuto04;

    private GameObject InstantiateVoice(Transform position)
    {
        return Instantiate(_voicePrefab3D, position.position, Quaternion.identity);
    }

    public void PlayVoice(VoiceType voicetype, float volume, Transform position)
    {
        GameObject voice = InstantiateVoice(position);
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

            case VoiceType.Tuto04:
                voiceSource.clip = _voiceTuto04;
                break;

            case VoiceType.HUB01:
                voiceSource.clip = _voiceHUB01;
                break;

            case VoiceType.HUB02:
                voiceSource.clip = _voiceHUB02;
                break;

            case VoiceType.HUB03:
                voiceSource.clip = _voiceHUB03;
                break;

            case VoiceType.HUB04:
                voiceSource.clip = _voiceHUB04;
                break;

            case VoiceType.HUB05:
                voiceSource.clip = _voiceHUB05;
                break;
        }

        voiceSource.volume = volume;
        voiceSource.Play();
    }
}

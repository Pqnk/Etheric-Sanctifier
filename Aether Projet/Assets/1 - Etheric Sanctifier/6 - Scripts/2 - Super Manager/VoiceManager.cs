using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VoiceType
{
    Intro,
    ChoiceTuto,
    ChoiceMission,
    BriefingMission,
    Tuto01,
    Tuto02,
    Tuto03,
    Tuto04
}

public class VoiceManager : MonoBehaviour
{
    [Header("Prefab Voice")]
    [SerializeField] private GameObject _voicePrefab3D;

    [Header("Voices HUB")]
    [SerializeField] private AudioClip _voiceIntro;
    [SerializeField] private AudioClip _voiceChoiceTuto;
    [SerializeField] private AudioClip _voiceChoiceMission;

    [Header("Voices Tutorial")]
    [SerializeField] private AudioClip _voiceTuto01;
    [SerializeField] private AudioClip _voiceTuto02;
    [SerializeField] private AudioClip _voiceTuto03;
    [SerializeField] private AudioClip _voiceTuto04;

    [Header("Voices Mission")]
    [SerializeField] private AudioClip _voiceBriefingMission;

    private GameObject InstantiateVoice(Transform position)
    {
        return Instantiate(_voicePrefab3D, position.position, Quaternion.identity);
    }

    public void PlayVoiceAtLocation(VoiceType voicetype, float volume, Transform position)
    {
        GameObject voice = InstantiateVoice(position);
        AudioSource voiceSource = voice.GetComponent<AudioSource>();

        switch (voicetype)
        {
            case VoiceType.Intro:
                voiceSource.clip = _voiceIntro;
                break;

            case VoiceType.BriefingMission:
                voiceSource.clip = _voiceBriefingMission;
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
        }

        voiceSource.volume = volume;
        voiceSource.Play();
    }

    public AudioClip GetVoice(VoiceType voiceType)
    {
        switch(voiceType)
        {
            case VoiceType.Intro:
                return _voiceIntro;

            case VoiceType.ChoiceMission:
                return _voiceChoiceMission;

            case VoiceType.ChoiceTuto:
                return _voiceChoiceTuto;

            case VoiceType.Tuto01:
                return _voiceTuto01;
        }

        return null;
    }
}

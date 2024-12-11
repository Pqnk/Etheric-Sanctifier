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
    TutoDemarrage,
    TutoYeux,
    TutoDos,
    TutoPieds,
    TutoTir,
    Victory,
    Defeat
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
    [SerializeField] private AudioClip _voiceTutoDemarrage;
    [SerializeField] private AudioClip _voiceTutoYeux;
    [SerializeField] private AudioClip _voiceTutoPieds;
    [SerializeField] private AudioClip _voiceTutoDos;
    [SerializeField] private AudioClip _voiceTutoTir;

    [Header("Voices Mission")]
    [SerializeField] private AudioClip _voiceBriefingMission;
    [SerializeField] private AudioClip _voiceFirstPalier;
    [SerializeField] private AudioClip _voiceSecondPalier;
    [SerializeField] private AudioClip _voiceVictory;
    [SerializeField] private AudioClip _voiceDefeat;

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


            case VoiceType.TutoDemarrage:
                voiceSource.clip = _voiceTutoDemarrage;
                break;

            case VoiceType.TutoYeux:
                voiceSource.clip = _voiceTutoYeux;
                volume = 0.2f;
                break;

            case VoiceType.TutoDos:
                voiceSource.clip = _voiceTutoDos;
                break;

            case VoiceType.TutoTir:
                voiceSource.clip = _voiceTutoTir;
                break;

            case VoiceType.TutoPieds:
                voiceSource.clip = _voiceTutoPieds;
                break;

            case VoiceType.Victory:
                voiceSource.clip = _voiceVictory;
                break;

            case VoiceType.Defeat:
                voiceSource.clip = _voiceDefeat;
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

            case VoiceType.TutoDemarrage:
                return _voiceTutoDemarrage;

            case VoiceType.TutoTir:
                return _voiceTutoTir;

            case VoiceType.TutoPieds:
                return _voiceTutoPieds;

            case VoiceType.TutoYeux:
                return _voiceTutoYeux;

            case VoiceType.TutoDos:
                return _voiceTutoDos;
 
            case VoiceType.Defeat:
                return _voiceDefeat;

            case VoiceType.Victory:
                return _voiceVictory;

        }

        return null;
    }
}

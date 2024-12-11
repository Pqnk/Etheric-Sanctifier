using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


public enum SoundType
{
    Music,
    HUB,
    MusicTuto,
    Collision,
    TeleportReady,
    Teleporting,
    TeleportValidated,
    TeleportCanceled,
    SearchingObjective,
    FindingObjective,
    ExplosionGoat,
    SlurpGoat,
    BeehGoat,
    TeleportAppearing
}


public class SoundManager : MonoBehaviour
{
    [Header("Prefab Sound")]
    [SerializeField] private GameObject _soundPrefab3D;
    [SerializeField] private GameObject _soundPrefab2D;

    [Header("Musics")]
    [SerializeField] private AudioClip _music;
    [SerializeField] private AudioClip _hub;
    [SerializeField] private AudioClip _tuto;
    private bool _isMusicPlaying = false;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _soundVFXCollision;
    [SerializeField] private AudioClip _soundVFXTeleportReady;
    [SerializeField] private AudioClip _soundVFXTeleporting;
    [SerializeField] private AudioClip _soundVFXTeleportValidated;
    [SerializeField] private AudioClip _soundVFXTeleportCanceled;
    [SerializeField] private AudioClip _soundVFXSearchingObjective;
    [SerializeField] private AudioClip _soundVFXFindingObjective;
    [SerializeField] private AudioClip _soundVFXExplosionGOAT;
    [SerializeField] private AudioClip _soundVFXSlurpGOAT;
    [SerializeField] private AudioClip _soundVFXSlurpGOATReverb;
    [SerializeField] private AudioClip _soundVFXBeehGOAT;
    [SerializeField] private AudioClip _soundVFXBeehGOATReverb;
    [SerializeField] private AudioClip _soundVFXTeleportAppearing;


    //  #################################################
    //  ######      MODULARY SOUND GENERATION      ######
    //  #################################################
    private void Start()
    {
    }

    private GameObject InstantiateSound()
    {
        return Instantiate(_soundPrefab2D);
    }
    private GameObject InstantiateSound3D(Vector3 target)
    {
        return Instantiate(_soundPrefab3D, target, Quaternion.identity);
    }

    public void PlaySound(SoundType soundtype, float volume)
    {
        GameObject sound = InstantiateSound();
        AudioSource soundSource = sound.GetComponent<AudioSource>();

        switch (soundtype)
        {
            case SoundType.Music:
                soundSource.clip = _music;
                soundSource.loop = true;
                _isMusicPlaying = true;
                break;

            case SoundType.MusicTuto:
                soundSource.clip = _tuto;
                soundSource.loop = true;
                _isMusicPlaying = true;
                break;


            case SoundType.HUB:
                soundSource.clip = _hub;
                soundSource.loop = true;
                _isMusicPlaying = true;
                break;

            case SoundType.Collision:
                soundSource.clip = _soundVFXCollision;
                break;

            case SoundType.TeleportReady:
                soundSource.clip = _soundVFXTeleportReady;
                break;

            case SoundType.Teleporting:
                soundSource.clip = _soundVFXTeleporting;
                volume = 0.2f;
                break;

            case SoundType.TeleportValidated:
                soundSource.clip = _soundVFXTeleportValidated;
                break;

            case SoundType.TeleportCanceled:
                soundSource.clip = _soundVFXTeleportCanceled;
                break;

            case SoundType.SearchingObjective:
                soundSource.clip = _soundVFXSearchingObjective;
                volume = 0.2f;
                break;

            case SoundType.FindingObjective:
                soundSource.clip = _soundVFXFindingObjective;
                volume = 0.2f;
                break;
        }

        soundSource.volume = volume;
        soundSource.Play();
    }
    public void PlaySoundAtLocation(SoundType soundtype, float volume, Vector3 targetPosition)
    {
        GameObject sound = InstantiateSound3D(targetPosition);
        AudioSource soundSource = sound.GetComponent<AudioSource>();

        switch (soundtype)
        {
            case SoundType.Music:
                if (!_isMusicPlaying)
                {
                    //sound.GetComponent<Destroy>().enabled = false;
                    soundSource.clip = _music;
                    soundSource.loop = true;
                    _isMusicPlaying = true;
                }
                else
                {
                    sound.GetComponent<Destroy>().countdown = 0.1f;
                }
                break;

            case SoundType.HUB:
                if (!_isMusicPlaying)
                {
                    //sound.GetComponent<Destroy>().enabled = false;
                    soundSource.clip = _hub;
                    soundSource.loop = true;
                    _isMusicPlaying = true;
                }
                else
                {
                    sound.GetComponent<Destroy>().countdown = 0.1f;
                }
                break;

            case SoundType.Collision:
                soundSource.clip = _soundVFXCollision;

                break;

            case SoundType.TeleportReady:
                soundSource.clip = _soundVFXTeleportReady;
                break;

            case SoundType.Teleporting:
                soundSource.clip = _soundVFXTeleporting;
                volume = 0.2f;
                break;

            case SoundType.TeleportValidated:
                soundSource.clip = _soundVFXTeleportValidated;
                break;

            case SoundType.TeleportCanceled:
                soundSource.clip = _soundVFXTeleportCanceled;
                break;

            case SoundType.SearchingObjective:
                soundSource.clip = _soundVFXSearchingObjective;
                volume = 0.2f;
                break;

            case SoundType.FindingObjective:
                soundSource.clip = _soundVFXFindingObjective;
                volume = 0.2f;
                break;
        }

        soundSource.volume = volume;
        soundSource.Play();
    }
}

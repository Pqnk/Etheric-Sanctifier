using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


public enum SoundType
{
    Music,
    HUBMusic,
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
    SlurpGoatReverb,
    BeehGoat,
    BeehGoatReverb,
    TeleportAppearing,
    Shoot,
    ShootBig,
    ShootImpact,
    ShootBigImpact,
    Sword,
    BigShootReady
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
    [SerializeField] private AudioClip _soundVFXShoot;
    [SerializeField] private AudioClip _soundVFXShootImpact;
    [SerializeField] private AudioClip _soundVFXShootBig;
    [SerializeField] private AudioClip _soundVFXShootBigImpact;
    [SerializeField] private AudioClip _soundVFXSwordImpact;
    [SerializeField] private AudioClip _soundVFXBigShootReady;


    //  #################################################
    //  ######      MODULARY SOUND GENERATION      ######
    //  #################################################
    private void Start()
    {
    }

    private GameObject InstantiateSound2D()
    {
        return Instantiate(_soundPrefab2D);
    }
    private GameObject InstantiateSound3D(Vector3 target)
    {
        return Instantiate(_soundPrefab3D, target, Quaternion.identity);
    }

    public void PlaySound(SoundType soundtype, float volume)
    {
        GameObject sound = InstantiateSound2D();
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


            case SoundType.HUBMusic:
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


            case SoundType.BigShootReady:
                soundSource.clip = _soundVFXBigShootReady;
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
                    soundSource.clip = _music;
                    soundSource.loop = true;
                    _isMusicPlaying = true;
                }
                else
                {
                    sound.GetComponent<Destroy>().countdown = 0.1f;
                }
                break;

            case SoundType.HUBMusic:
                if (!_isMusicPlaying)
                {
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
                break;

            case SoundType.TeleportAppearing:
                soundSource.clip = _soundVFXTeleportAppearing;
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

            case SoundType.Shoot:
                soundSource.clip = _soundVFXShoot;
                break;

            case SoundType.ShootImpact:
                soundSource.clip = _soundVFXShootImpact;
                break;

            case SoundType.ShootBig:
                soundSource.clip = _soundVFXShootBig;
                break;

            case SoundType.ShootBigImpact:
                soundSource.clip = _soundVFXShootBigImpact;
                break;

            case SoundType.BeehGoat:
                soundSource.clip = _soundVFXBeehGOAT;
                break;

            case SoundType.BeehGoatReverb:
                soundSource.clip = _soundVFXBeehGOATReverb;
                break;

            case SoundType.SlurpGoat:
                soundSource.clip = _soundVFXSlurpGOAT;
                break;

            case SoundType.SlurpGoatReverb:
                soundSource.clip = _soundVFXSlurpGOATReverb;
                break;

            case SoundType.Sword:
                soundSource.clip = _soundVFXSwordImpact;
                break;

            case SoundType.BigShootReady:
                soundSource.clip = _soundVFXBigShootReady;
                break;
        }

        soundSource.volume = volume;
        soundSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System;
using VHierarchy.Libs;

public enum SoundType
{
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
    BigShootReady,
    Slap,
    Ouch,
    Death,
    Horn
}


public class SoundManager : MonoBehaviour
{

    [Header("List Of All SoundPrefab Instanciated")]
    [SerializeField] private List<GameObject> _listOfAllSoundPrefabs;

    [Header("Prefab Sound")]
    [SerializeField] private GameObject _soundPrefab3D;

    [Header("Sound Effects")]
    #region AudioClips
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
    [SerializeField] private AudioClip _soundVFXSlap;
    [SerializeField] private AudioClip _soundVFXOuch;
    [SerializeField] private AudioClip _soundVFXDeath;
    [SerializeField] private AudioClip _soundVFXHorn;
    #endregion



    //  #################################################
    //  ######      MODULARY SOUND GENERATION      ######
    //  #################################################

    private GameObject InstantiateSound3D(Vector3 target)
    {
        GameObject s = Instantiate(_soundPrefab3D, target, Quaternion.identity);
        _listOfAllSoundPrefabs.Add(s);
        s.GetComponent<SoundPrefab3D>().OnAudioFinished += DestroySoundPrefab3D;
        return s;
    }

    private void DestroySoundPrefab3D(GameObject s)
    {
        _listOfAllSoundPrefabs.Remove(s);
        Destroy(s);
    }

    public void DestroyAllSoundsPrefabs3D()
    {
        _listOfAllSoundPrefabs.Clear();
    }

    public void PlaySoundAtLocation(SoundType soundtype, float volume, Vector3 targetPosition)
    {
        GameObject sound = InstantiateSound3D(targetPosition);
        AudioSource soundSource = sound.GetComponent<AudioSource>();

        switch (soundtype)
        {
            case SoundType.Horn:
                soundSource.clip = _soundVFXHorn;
                break;

            case SoundType.Death:
                soundSource.clip = _soundVFXDeath;
                break;

            case SoundType.Ouch:
                soundSource.clip = _soundVFXOuch;
                break;

            case SoundType.Slap:
                soundSource.clip = _soundVFXSlap;
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

        sound.GetComponent<SoundPrefab3D>().StartWaitingEndOfSound();
    }

    public void UpdatePitchAllSounds3D(float pitch)
    {
        foreach(GameObject g in _listOfAllSoundPrefabs)
        {
            g.GetComponent<AudioSource>().pitch = pitch;
        }
    }
}

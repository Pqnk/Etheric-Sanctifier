using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundPrefab3D : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public event Action<GameObject> OnAudioFinished;

    public void StartWaitingEndOfSound()
    {
        StartCoroutine(WaitForAudioToEnd());
    }

    IEnumerator WaitForAudioToEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        OnAudioFinished?.Invoke(this.transform.gameObject);
    }
}

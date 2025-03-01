using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : MonoBehaviour
{
    [SerializeField] private AudioSource _vinylSource;
    [SerializeField] private List<AudioClip> _vinylMusics;
    private int currentIndex = 0;

    void Start()
    {
        _vinylSource = GetComponent<AudioSource>();
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandNaked"))
        {
            SwitchToNext();
        }
    }

    private void SwitchToNext()
    {
        if (_vinylMusics.Count == 0) return;

        _vinylSource.Stop();
        currentIndex = (currentIndex + 1) % _vinylMusics.Count;
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource.Play();
    }

    public void UpdatePitchMusic(float pitch)
    {
        _vinylSource.pitch = pitch;
    }
}

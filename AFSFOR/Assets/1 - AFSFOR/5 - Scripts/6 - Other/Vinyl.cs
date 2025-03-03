using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vinyl : MonoBehaviour
{
    [SerializeField] private AudioSource _vinylSource;
    [SerializeField] private List<AudioClip> _vinylMusics;
    private int currentIndex = 0;
    private bool _isVinylActive = false;
    [SerializeField] private TMP_Text _textUIVinyl;

    void Start()
    {
        _vinylSource = GetComponent<AudioSource>();
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource?.Play();
        _isVinylActive = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandNaked"))
        {
            if (_isVinylActive)
            {
                SwitchToNext();
            }
            else
            {
                _vinylSource?.Play();
                _isVinylActive = true;
            }
        }
        if (other.CompareTag("Sword"))
        {
            ToggleVinylMusic();
        }

        UpdateTextUIVinyl();
    }

    private void SwitchToNext()
    {
        if (_vinylMusics.Count == 0) return;

        _vinylSource.Stop();
        currentIndex = (currentIndex + 1) % _vinylMusics.Count;
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource.Play();
    }

    public void UpdatePitchMusic(float newPitch)
    {
        if (newPitch < 0.2)
        {
            newPitch *= 5;
        }
        _vinylSource.pitch = newPitch;
    }

    public void ToggleVinylMusic()
    {
        if (_isVinylActive)
        {
            _vinylSource?.Stop();
            _isVinylActive = false;
        }
        else
        {
            _vinylSource?.Play();
            _isVinylActive = true;
        }
    }

    private void UpdateTextUIVinyl()
    {
        if (_isVinylActive)
        {
            _textUIVinyl.text = _vinylMusics[currentIndex].name;
        }
        else
        {
            _textUIVinyl.text = "Song title here...";
        }
    }
}

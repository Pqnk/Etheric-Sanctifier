using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vinyl : MonoBehaviour
{
    [SerializeField] private AudioSource _vinylSource;
    [SerializeField] private List<AudioClip> _vinylMusics;
    private int currentIndex = 0;
    [SerializeField] private bool _isVinylActive = false;
    [SerializeField] private TMP_Text _textUIVinyl;

    void Start()
    {
        _vinylSource = GetComponent<AudioSource>();
        _vinylSource.clip = _vinylMusics[currentIndex];
        UpdateTextUIVinyl();
    }

    public void PlayOrStop()
    {
        if(_isVinylActive)
        {
            _vinylSource.Stop();
        }
        else
        {
            _vinylSource.Play();
        }
    }

    public void SwitchToNext()
    {
        if (_vinylMusics.Count == 0) return;


        SuperManager.instance.vfxManager.InstantiateVFX_vfxSwordImpact(this.transform.position);
        _vinylSource.Stop();
        currentIndex = (currentIndex + 1) % _vinylMusics.Count;
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource.Play();
        UpdateTextUIVinyl();
    }

    public void SwitchToPrevious()
    {
        if (_vinylMusics.Count == 0) return;

        SuperManager.instance.vfxManager.InstantiateVFX_vfxSwordImpact(this.transform.position);
        _vinylSource.Stop();
        currentIndex = (currentIndex - 1 + _vinylMusics.Count) % _vinylMusics.Count;
        _vinylSource.clip = _vinylMusics[currentIndex];
        _vinylSource.Play();
        UpdateTextUIVinyl();
    }

    public void UpdatePitchMusic(float newPitch)
    {
        if (newPitch < 0.2)
        {
            newPitch *= 7;
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
            _textUIVinyl.text = "Touch with hand to play ! ";
        }
    }
}

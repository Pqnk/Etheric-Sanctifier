using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeScaleManager : MonoBehaviour
{
    [SerializeField] private float _pauseTimeScale = 0.1f;
    [SerializeField] private float _slowMoDuration = 0.5f;
    public bool slowMoActive = false;

    [SerializeField] private Vinyl _vinylRef;

    private Coroutine _coroutine;
    private GameObject _ppBlackAndWhite;
    public static event Action OnSlowMoFinished;

    private void Awake()
    {
        _ppBlackAndWhite = transform.GetChild(0).gameObject;
    }

    public void ToggleSlowMotion(bool activate)
    {
        if (activate)
        {
            slowMoActive = true;
            _ppBlackAndWhite.SetActive(true);
            Time.timeScale = _pauseTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            UpdatePitchSounds();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _coroutine = StartCoroutine(DeactivateSlowMotionWithDelay());
        }
        else
        {
            slowMoActive = false;
            _ppBlackAndWhite.SetActive(false);
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            UpdatePitchSounds();
        }
    }

    IEnumerator DeactivateSlowMotionWithDelay()
    {
        yield return new WaitForSecondsRealtime(_slowMoDuration);
        ToggleSlowMotion(false);
        OnSlowMoFinished?.Invoke();
    }

    private void UpdatePitchSounds()
    {
        SuperManager.instance.soundManager.UpdatePitchAllSounds3D(Time.timeScale);
        _vinylRef.UpdatePitchMusic(Time.timeScale);
    }

}

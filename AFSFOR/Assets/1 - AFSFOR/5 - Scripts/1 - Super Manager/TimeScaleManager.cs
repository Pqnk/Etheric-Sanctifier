using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    [SerializeField] private float _pauseTimeScale = 0.1f;
    [SerializeField] private float _slowMoDuration = 1.0f;
    [SerializeField] private bool _slowMoActive = false;

    private GameObject _ppBlackAndWhite;

    private void Awake()
    {
        _ppBlackAndWhite = transform.GetChild(0).gameObject;
    }

    public void ToggleSlowMotion(bool activate)
    {
        if (activate)
        {
            if(_slowMoActive)
            {
                StopCoroutine(DeactivateSlowMotionWithDelay());
            }
            _slowMoActive = true;
            _ppBlackAndWhite.SetActive(true);
            Time.timeScale = _pauseTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            StartCoroutine(DeactivateSlowMotionWithDelay());
        }
        else
        {
            _slowMoActive = false;
            _ppBlackAndWhite.SetActive(false);
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    IEnumerator DeactivateSlowMotionWithDelay()
    {
        yield return new WaitForSecondsRealtime(_slowMoDuration);
        ToggleSlowMotion(false);
    }

}

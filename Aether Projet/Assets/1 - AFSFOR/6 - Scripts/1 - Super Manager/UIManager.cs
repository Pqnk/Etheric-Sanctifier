using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("PostProcess")]
    [SerializeField] private GameObject _postProcessVolume;
    [SerializeField] float minExposure = -10.0f;
    [SerializeField] float maxExposure = -0.5f;
    [SerializeField] float maxExposureLevel1 = -2.5f;
    [SerializeField] float transitionSpeed = 1.0f;

    private Volume _volume;
    private ColorAdjustments _colorAdjustments;
    private bool isIncreasing = true;
    float targetExposure;
    float currentExposure;

    private GameManagerAFSFOR _gM;
    private LevelManager _lM;

    private void Start()
    {
        _gM = SuperManager.instance.gameManagerAFSFOR;
        _lM = SuperManager.instance.levelManager;
        GetPostProcessVolumeInScene(LevelType.HUB);
    }

    private void Update()
    {
    }

    public void GetPostProcessVolumeInScene(LevelType levelType)
    {
        _postProcessVolume = _lM.FindInScene(levelType, "--LIGHTING--");
        _volume = _postProcessVolume.transform.GetChild(2).transform.GetChild(1).GetComponent<Volume>();
        BlackToVisible();
    }
    public void VisibleToBlack()
    {
        if (_volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
        {
            targetExposure = minExposure;

            if (_lM.GetCurrentLevelType() == LevelType.Level01)
            {
                currentExposure = maxExposureLevel1;
            }
            else
            {
                currentExposure = maxExposure;
            }
            StartCoroutine(InterpolateExposure());
        }
        else
        {
            Debug.LogError("Color Adjustments effect not found in the Volume profile!");
        }
    }
    public void BlackToVisible()
    {
        if (_volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
        {
            currentExposure = minExposure;

            if (_lM.GetCurrentLevelType() == LevelType.Level01)
            {
                targetExposure = maxExposureLevel1;
            }
            else
            {
                targetExposure = maxExposure;

            }
            StartCoroutine(InterpolateExposure());
        }
        else
        {
            Debug.LogError("Color Adjustments effect not found in the Volume profile!");
        }
    }
    public IEnumerator InterpolateExposure()
    {
        _colorAdjustments.postExposure.value = currentExposure;

        while (!Mathf.Approximately(currentExposure, targetExposure))
        {
            currentExposure = Mathf.MoveTowards(
                currentExposure,
                targetExposure,
                transitionSpeed * Time.deltaTime
            );
            _colorAdjustments.postExposure.value = currentExposure;
            yield return null;
        }

    }
}

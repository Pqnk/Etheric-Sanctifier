using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;

public class Radar : MonoBehaviour
{
    private GameObject _objectiveParent;
    private AudioSource _radarSource;
    private int _objectiveIndex = 0;
    private bool _isRadarActive = false;
    private bool _canPlay = true;

    //  For testing
    public float maxVolume = 1.0f;
    public float minVolume = 0.0f;
    public float angleThreshold = 90f;
    public bool isTest = false;
    [Range(0f, 500f)] public float distanceMaxToDetect = 10;
    public float maxFrequency = 2.0f;
    public float minFrequency = 0.2f;

    void Start()
    {
        _radarSource = GetComponent<AudioSource>();
        _objectiveParent = SuperManager.instance.gameManagerAetherPunk.artefactParent;
    }
    void Update()
    {
        if (SuperManager.instance.tutoManager.isDetectionLearned)
        {
            CalculateRadarFrequencyInRelationToDistance();
            CalculateVolumeInRelationToOrientation();
        }
    }

    //  ###########################################################
    //  #############       RADAR FUNCTIONS         ###############
    //  ###########################################################
    private void CalculateVolumeInRelationToOrientation()
    {
        Vector3 directionToTarget = (_objectiveParent.transform.GetChild(_objectiveIndex).transform.position - this.gameObject.transform.position).normalized;
        float dotProduct = Vector3.Dot(this.gameObject.transform.forward, directionToTarget);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        float volume = Mathf.Lerp(maxVolume, minVolume, Mathf.Clamp01(angle / angleThreshold));
        _radarSource.volume = volume;
    }
    private void CalculateRadarFrequencyInRelationToDistance()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, _objectiveParent.transform.GetChild(_objectiveIndex).transform.position);
        if (distance < distanceMaxToDetect && _canPlay)
        {
            _canPlay = false;
            float frequency = Mathf.Lerp(minFrequency, maxFrequency, Mathf.Clamp01(distance / distanceMaxToDetect));
            //Debug.Log("Frequency : " + frequency);
            StartCoroutine(PlayRadar(frequency));

        }
    }
    IEnumerator PlayRadar(float frequency)
    {
        yield return new WaitForSeconds(frequency);

        _radarSource.Play();
        _canPlay = true;
    }
    public void UpdateIndexObjective()
    {
        if (_objectiveIndex < 3)
        {
            _objectiveIndex++;
        }
    }

    public void ToggleRadar(bool newActivation)
    {
        _isRadarActive = newActivation;
    }
}

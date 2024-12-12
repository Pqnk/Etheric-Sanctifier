using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarUpgraded : MonoBehaviour
{
    [Header("Detection Parameters")]
    [SerializeField] private List<Ghost> _ghostBehindList;
    [SerializeField] private Ghost _currentNearestGhost;
    [SerializeField] private float _maxDetectionDistance = 25.0f;

    [Header("Sound Parameters")]
    [SerializeField] private AudioSource _radarSource;
    [SerializeField] private float maxVolume = 1.0f;
    [SerializeField] private float minVolume = 0.0f;
    [SerializeField] private float maxFrequency = 2.0f;
    [SerializeField] private float minFrequency = 0.2f;

    private bool _canPlay = true;

    //  ###########################################
    //  ##########  RADAR START UPDATE  ###########
    //  ###########################################
    private void Start()
    {
        _radarSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        EstimateCloserGhost();
        CalculateVolumeInRelationToOrientation();
        CalculateRadarFrequencyInRelationToDistance();
    }

    //  ###########################################
    //  ############  RADAR TRIGGERS  #############
    //  ###########################################
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            _ghostBehindList.Add(other.GetComponent<Ghost>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            foreach (Ghost g in _ghostBehindList)
            {
                if (other.GetComponent<Ghost>() == g)
                {
                    _ghostBehindList.Remove(g);
                }
            }
        }
    }


    //  ###########################################
    //  #######  RADAR DETECTION AND VOLUME  ######
    //  ###########################################
    private void EstimateCloserGhost()
    {
        if (_ghostBehindList != null)
        {
            float closestDistance = Mathf.Infinity;
            Ghost nearestGhost = null;

            foreach (Ghost g in _ghostBehindList)
            {
                float distanceToGhost = Vector3.Distance(this.gameObject.transform.position, g.transform.position);

                if (distanceToGhost < closestDistance)
                {
                    closestDistance = distanceToGhost;
                    nearestGhost = g;
                }
            }
            _currentNearestGhost = nearestGhost;
        }
    }
    private void CalculateVolumeInRelationToOrientation()
    {
        if (_currentNearestGhost != null)
        {
            float distanceToTarget = Vector3.Distance(_currentNearestGhost.transform.position, this.gameObject.transform.position);
            float volume = 1 - distanceToTarget / _maxDetectionDistance;
            _radarSource.volume = volume;
        }
    }
    private void CalculateRadarFrequencyInRelationToDistance()
    {
        if (_currentNearestGhost != null)
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, _currentNearestGhost.transform.position);
            if (_canPlay)
            {
                _canPlay = false;
                float frequency = Mathf.Lerp(minFrequency, maxFrequency, Mathf.Clamp01(distance / _maxDetectionDistance));
                StartCoroutine(PlayRadar(frequency));
            }
        }
    }
    IEnumerator PlayRadar(float frequency)
    {
        yield return new WaitForSeconds(frequency);

        _radarSource.Play();
        _canPlay = true;
    }

}

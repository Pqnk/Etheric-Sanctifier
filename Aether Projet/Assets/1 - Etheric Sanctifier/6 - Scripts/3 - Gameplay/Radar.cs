using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;

public class Radar : MonoBehaviour
{
    private AudioSource _radarSource;
    private bool _isRadarActive = false;
    private bool _canPlay = true;

    //  For testing
    public float maxVolume = 1.0f;
    public float minVolume = 0.0f;
    public float angleThreshold = 90f;
    public bool isTest = false;
    [Range(0f, 500f)] public float distanceMaxToDetect = 50;
    public float maxFrequency = 2.0f;
    public float minFrequency = 0.2f;

    private Transform currentNearestGhost;

    [SerializeField] private float detectionRadius = 50f;
    [SerializeField] private float detectionAngle = 120f;

    void Start()
    {
        _radarSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (_isRadarActive && _canPlay)
        {
            DetectNearestGhostBehind();
            CalculateRadarFrequencyInRelationToDistance();
            CalculateVolumeInRelationToOrientation();
        }
    }
    
    private void DetectNearestGhostBehind()
    {
        float closestDistance = Mathf.Infinity;
        Transform nearestGhost = null;

        foreach (Transform ghost in SuperManager.instance.ghostManager.allGhosts)
        {
            Vector3 directionToGhost = (ghost.position - this.gameObject.transform.position).normalized;

            float distanceToGhost = Vector3.Distance(this.gameObject.transform.position, ghost.position);
            if (distanceToGhost > detectionRadius) continue;

            float angleToGhost = Vector3.Angle(-this.gameObject.transform.forward, directionToGhost);
            float dotProduct = Vector3.Dot(-this.gameObject.transform.forward, directionToGhost);

            if (angleToGhost <= detectionAngle / 2)
            {
                if (distanceToGhost < closestDistance)
                {
                    closestDistance = distanceToGhost;
                    nearestGhost = ghost;
                }
            }
        }

        currentNearestGhost = nearestGhost;
    }

    private void CalculateVolumeInRelationToOrientation()
    {
        Vector3 directionToTarget = (currentNearestGhost.position - this.gameObject.transform.position).normalized;
        float dotProduct = Vector3.Dot(-this.gameObject.transform.forward, directionToTarget);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        float volume = Mathf.Lerp(maxVolume, minVolume, Mathf.Clamp01(angle / angleThreshold));
        _radarSource.volume = volume;
    }
    private void CalculateRadarFrequencyInRelationToDistance()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, currentNearestGhost.position);
        if (distance < distanceMaxToDetect && _canPlay)
        {
            _canPlay = false;
            float frequency = Mathf.Lerp(minFrequency, maxFrequency, Mathf.Clamp01(distance / distanceMaxToDetect));
            StartCoroutine(PlayRadar(frequency));

        }
    }
    IEnumerator PlayRadar(float frequency)
    {
        yield return new WaitForSeconds(frequency);

        _radarSource.Play();
        _canPlay = true;
    }

}

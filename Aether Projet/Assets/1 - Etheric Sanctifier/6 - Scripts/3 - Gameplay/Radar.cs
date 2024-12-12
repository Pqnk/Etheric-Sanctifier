using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Radar : MonoBehaviour
{
    private AudioSource _radarSource;
    private  bool _isRadarActive = false;
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
    private float detectionAngleUpdate = 0;

    void Start()
    {
        _radarSource = GetComponent<AudioSource>();
        ToggleRadar(true);
        detectionAngleUpdate = 180 - detectionAngle / 2;
    }
    
    void Update()
    {
        
        Debug.DrawRay(transform.position, new Vector3(Mathf.Sin((transform.eulerAngles.y + detectionAngleUpdate) * Mathf.PI / 180), 0, Mathf.Cos((transform.eulerAngles.y + detectionAngleUpdate) * Mathf.PI / 180)));
        Debug.DrawRay(transform.position, new Vector3(Mathf.Sin((transform.eulerAngles.y - detectionAngleUpdate) * Mathf.PI / 180), 0, Mathf.Cos((transform.eulerAngles.y - detectionAngleUpdate) * Mathf.PI / 180)));


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
        Vector3 orientationX = new Vector3(Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180));
        Vector3 orientationZ = new Vector3(Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180));

        foreach (Transform ghost in SuperManager.instance.ghostManager.allGhosts)
        {
            float distanceToGhost = Vector3.Distance(this.gameObject.transform.position, ghost.position);
            if (distanceToGhost > detectionRadius)  continue;

            Vector3 directionToGhost = ghost.position - this.gameObject.transform.position;


            float angleToGhostX = Mathf.Acos((directionToGhost.x * orientationX.x + directionToGhost.z * orientationX.z) / (Mathf.Sqrt(Mathf.Pow(directionToGhost.x,2)+Mathf.Pow(directionToGhost.z,2))))*180/Mathf.PI;
            float angleToGhostZ = Mathf.Acos((directionToGhost.x * orientationZ.x + directionToGhost.z * orientationZ.z) / (Mathf.Sqrt(Mathf.Pow(directionToGhost.x, 2) + Mathf.Pow(directionToGhost.z, 2)))) * 180 / Mathf.PI;
            //Debug.Log(directionToGhost);
            //Debug.Log(angleToGhostX);
            //Debug.Log(angleToGhostZ);

            //float dotProduct = Vector3.Dot(-this.gameObject.transform.forward, directionToGhost);

            if (angleToGhostZ >= detectionAngleUpdate)
            {
                //Debug.Log("Vibration");
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
        if (currentNearestGhost != null)
        {
            float distanceToTarget = Vector3.Distance(currentNearestGhost.position, this.gameObject.transform.position);
            float volume = 1 - distanceToTarget / detectionRadius;
            _radarSource.volume = volume;
        } 
    }
    private void CalculateRadarFrequencyInRelationToDistance()
    {
        if (currentNearestGhost != null)
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, currentNearestGhost.position);
            if (distance < distanceMaxToDetect && _canPlay)
            {
                _canPlay = false;
                float frequency = Mathf.Lerp(minFrequency, maxFrequency, Mathf.Clamp01(distance / distanceMaxToDetect));
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

    public void ToggleRadar(bool state)
    {
        _isRadarActive = state;
    }
}

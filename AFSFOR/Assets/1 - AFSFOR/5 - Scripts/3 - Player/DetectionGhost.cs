using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionGhost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Enemy behaviorGhost = other.gameObject.GetComponent<Enemy>();
            behaviorGhost.isDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Enemy behaviorGhost = other.gameObject.GetComponent<Enemy>();
            behaviorGhost.isDetected = false;
        }
    }
}

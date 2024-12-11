using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionGhost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Ghost behaviorGhost = other.gameObject.GetComponent<Ghost>();
            behaviorGhost.SetIsDetected(true);
        }
    }
}

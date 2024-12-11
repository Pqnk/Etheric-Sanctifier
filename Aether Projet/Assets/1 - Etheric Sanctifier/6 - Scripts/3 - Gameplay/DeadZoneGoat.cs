using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class DeadZoneGoat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            other.gameObject.GetComponent<Ghost>().KillAndDestroyGhost();
        }
    }
}

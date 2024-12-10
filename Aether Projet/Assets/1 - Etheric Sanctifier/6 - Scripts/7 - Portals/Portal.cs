using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;
    public bool isPortalActive = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            isPortalActive = false;
            SuperManager.instance.levelManager.LoadLevel(_levelDestination);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;
    [SerializeField] private bool _isPortalActive = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            SuperManager.instance.levelManager.LoadLevel(_levelDestination);
            _isPortalActive = false;
        }
    }
}

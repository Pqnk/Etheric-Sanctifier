using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TriggerCheckPoint : MonoBehaviour
{
    public GameObject checkPoint;

    void Awake()
    {
        checkPoint.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkPoint.SetActive(true);
        }
    }
}

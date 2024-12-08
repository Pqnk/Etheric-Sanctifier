using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Target : MonoBehaviour
{
    public bool canTP = true;

    private void Update()
    {

    }

    /*public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Forbidden")
        {
            canTP = false;
            Debug.Log("STOP");
        }
    }*/


    //WARNING : named the forbidden zone's tag as "Forbidden"

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Forbidden")
        {
            canTP = false;
            Debug.Log("STOP");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forbidden")
        {
            canTP = true;
            Debug.Log("OK TP");
        }
    }
}

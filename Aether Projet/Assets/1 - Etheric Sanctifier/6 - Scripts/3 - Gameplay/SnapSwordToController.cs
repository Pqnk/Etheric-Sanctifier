using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapSwordToController : MonoBehaviour
{
    public GameObject sword;

    void Start()
    {
        if (sword != null)
        {
            sword.transform.SetParent(this.transform);

        }
        else
        {
            Debug.LogWarning("No boot found ! Make sure you've put a boot in the public field.");
        }
    }

}

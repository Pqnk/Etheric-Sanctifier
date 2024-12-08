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
            Debug.LogWarning("No sword found ! Make sure you've put a sword in the public field.");
        }
    }

}

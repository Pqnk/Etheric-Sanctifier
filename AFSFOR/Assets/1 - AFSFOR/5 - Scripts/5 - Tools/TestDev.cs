using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDev : MonoBehaviour
{
    [SerializeField] bool isTest;

    Transform searchTarget;

    void Awake()
    {
        if (isTest)
        {
            searchTarget = GameObject.Find("--Player_AFSFOR--").transform;
            GetComponent<Enemy>().target = searchTarget;
        }
    }

}

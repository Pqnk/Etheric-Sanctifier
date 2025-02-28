using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (target == null) return;
        transform.LookAt(target);
    }
}

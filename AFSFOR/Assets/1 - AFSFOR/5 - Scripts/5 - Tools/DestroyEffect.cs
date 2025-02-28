using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [Header("Countdown 'till destruction'")]
    [Range(0f, 10f)]
    public float countdown = 2.0f;

    private void Start()
    {
        StartCoroutine(DestroyThisGameobject());
    }

    IEnumerator DestroyThisGameobject()
    {
        yield return new WaitForSeconds(countdown);

        Destroy(this.gameObject);
    }
}

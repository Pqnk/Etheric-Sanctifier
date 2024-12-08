using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [Header("Countdown 'till destruction")]
    [Range(0f,10f)] 
    public float countdown = 2.0f;

    void Start()
    {
        StartCoroutine(DestroyThisGameobject());
    }

    IEnumerator DestroyThisGameobject()
    {
        yield return new WaitForSeconds(countdown);

        Destroy(this.gameObject);
    }
}

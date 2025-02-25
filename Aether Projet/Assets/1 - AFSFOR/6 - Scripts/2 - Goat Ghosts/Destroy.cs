using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [Header("Countdown 'till destruction")]
    [Range(0f,10f)] 
    public float countdown = 2.0f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(DestroyThisGameobject());
    }

    private void Update()
    {
        if (!audioSource.isPlaying && audioSource.time > 0f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyThisGameobject()
    {
        yield return new WaitForSeconds(countdown);

        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class s_DetectionEffect : MonoBehaviour
{
    public s_DetectionColision detectionColision;

    public float timeBeforeDestroy;
    public float augmTime = 2f;
    public float dimTime = 2f;
    private bool isShrinking = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * 0.01f;

        StartCoroutine(GrossirSpotlight());
    }



    IEnumerator GrossirSpotlight()
    {
        float elapsedTime = 0f;

        while (elapsedTime < augmTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, Vector3.one, elapsedTime / augmTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(timeBeforeDestroy);

        TriggerShrink();
    }

    IEnumerator DiminutionSpotlight()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dimTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * -1, elapsedTime / augmTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * -1f;

        if (detectionColision != null)
        {
            detectionColision.RemoveEffect(this.gameObject);
        }
        Destroy(this.gameObject);
    }

    public void TriggerShrink()
    {
        if (!isShrinking)
        {
            isShrinking = true;
            StartCoroutine(DiminutionSpotlight());
        }
    }
}

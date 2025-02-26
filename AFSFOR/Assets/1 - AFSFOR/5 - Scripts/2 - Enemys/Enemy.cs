using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vie de l'enemy")]
    [SerializeField] int life = 10;
    public int currentLife;

    [Header("Vie de l'enemy")]
    [SerializeField] float speed = 1;
    public float currentSpeed;

    public bool idDead = false;
    private float scaleDuration = 1.0f;

    public void Get_Hit(int hit)
    {
        currentLife -= hit;

        if (currentLife <= 0)
        {
            currentLife = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator ScaleDownGhostAndDestroy()
    {
        Vector3 initialScalelle = transform.localScale;
        float elapsedTime = 0f;
        float startValue = 1f;
        float endValue = 0f;
        while (elapsedTime < scaleDuration)
        {
            float t = elapsedTime / scaleDuration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            transform.localScale = initialScalelle * currentValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        //PlayVFXKillGhost();
        //gM.RemoveGhostFromListAndDestroy(GetId());
    }
}

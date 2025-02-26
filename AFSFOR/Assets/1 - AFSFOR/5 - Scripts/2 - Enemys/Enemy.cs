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

    [HideInInspector] public bool idDead = false;
    [HideInInspector] public Rigidbody rb;

    SoundManager sM;

    private float scaleDuration = 1.0f;

    private void Awake()
    {
        currentLife = life;
        currentSpeed = speed;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        sM = SuperManager.instance.soundManager;      
    }

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
        currentSpeed = 0;
        StartCoroutine(ScaleDownGhostAndDestroy());
    }

    private IEnumerator ScaleDownGhostAndDestroy()
    {
        // Joue le son de la mort ------------------------------------------------------
        SoundType s;
        s = SoundType.BeehGoatReverb;
        sM.PlaySoundAtLocation(s, 0.5f, this.transform.position);

        // Reduit la taille du mesh ------------------------------------------------------
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

        // Joue le VFX de la mort ------------------------------------------------------
        PlayVFXKillGhost();
        Destroy(gameObject);
    }

    private void PlayVFXKillGhost()
    {
        SuperManager.instance.vfxManager.InstantiateVFX_vfxDeadGhostImpact(this.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Get_Hit(1);
        }
    }
}

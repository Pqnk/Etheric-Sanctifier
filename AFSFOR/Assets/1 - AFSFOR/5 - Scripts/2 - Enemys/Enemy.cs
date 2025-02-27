using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vie de l'enemy")]
    [SerializeField] int _life = 10;
    public int currentLife;

    [Header("Vie de l'enemy")]
    [SerializeField] float _speed = 1;
    public float currentSpeed;

    [Header("Vie de l'enemy")]
    [SerializeField] float _damage = 1;
    public float currentDamage;

    [Header("Info")]
    public float stopDistance = 2.5f;

    [HideInInspector] public bool idDead = false;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public SoundManager sM;

    private float scaleDuration = 1.0f;

    #region Get / Set
    public int GetLife()
    {
        return _life;
    }
    public float GetSpeed()
    {
        return _speed;
    }
    #endregion

    private void Awake()
    {
        currentLife = _life;
        currentSpeed = _speed;
        currentDamage = _damage;

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
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        // Joue le son de la mort --------------------------------------------------------
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
        SuperManager.instance.vfxManager.InstantiateVFX_vfxDeadGhostImpact(this.transform);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Get_Hit(1);
        }
    }
}

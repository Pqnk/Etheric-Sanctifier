using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stat de l'enemy")]
    [SerializeField] int _life = 10;
    public int currentLife;
    [SerializeField] float _speed = 1;
    public float currentSpeed;
    public float rotationSpeed = 2;
    [SerializeField] float _damage = 1;
    public float currentDamage;

    [Header("Info")]
    public float stopDistance = 2.5f;
    public bool getForceBack = false;
    public bool canLightDetected = false;

    [Header("Materials")]
    [SerializeField] private Material _baseMat;
    [SerializeField] private Material _emissiveMatDetection;
    [SerializeField] private Material _emissiveMatDamage;

    [HideInInspector] public bool idDead = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Transform target;
    [HideInInspector] public Player scriptPlayer;

    [HideInInspector] public SoundManager sM;

    private float scaleDuration = 1.0f;
    private bool isTakingDamage = false;
    private bool isDetected = false;
    private Renderer ghostRenderer;
    private float timerTakingDamage = -1;
    private float timerIsDetected = -1;

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

        if (this.gameObject.name == "HeadBoss")
        {
            ghostRenderer = transform.GetChild(0).GetComponentInChildren<Renderer>();
        }
        else
        {
            ghostRenderer = GetComponentInChildren<Renderer>();
        }

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        scriptPlayer = target.GetComponent<Player>();
        sM = SuperManager.instance.soundManager;
    }

    public void Get_Hit(int hit)
    {
        currentLife -= hit;
        SetIsTakingDamage(true);

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

        if (timerTakingDamage <= Time.time)
        {
            SetIsTakingDamage(false);
        }

        if (ghostRenderer)
        {
            CheckChangeMaterial();
        }
    }


    public void AddForceBack(float force, ForceMode forceMode)
    {
        if (getForceBack)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.AddForce(-direction * force, forceMode);
        }
    }

    #region Changement Couleur
    public void CheckChangeMaterial()
    {
        if (isTakingDamage)
        {
            ghostRenderer.material = _emissiveMatDamage;
        }
        else if (isDetected)
        {
            ghostRenderer.material = _emissiveMatDetection;
        }
        else
        {
            ghostRenderer.material = _baseMat;
        }
    }

    private void SetIsTakingDamage(bool v)
    {
        isTakingDamage = v;

        if (isTakingDamage == true)
        {
            timerTakingDamage = Time.time + 0.3f;
        }
    }

    public void Set_BaseMat(Material newMat)
    {
        _baseMat = newMat;
    }
    #endregion
}

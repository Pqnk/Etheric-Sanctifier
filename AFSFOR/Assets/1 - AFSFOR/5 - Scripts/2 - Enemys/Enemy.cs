using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    int _life = 10;
    int _damage = 1;

    [Header("Enemy Stats")]
    public int currentLife;
    [SerializeField] float _speed = 1;
    public float currentSpeed;
    public float rotationSpeed = 2;
    public float currentDamage;

    [Header("Info")]
    public float stopDistance = 2.5f;
    public bool getForceBack = false;
    public bool canLightDetected = false;

    [Header("Materials")]
    [SerializeField] private Material _baseMat;
    [SerializeField] private Material _emissiveMatDetection;
    [SerializeField] private Material _emissiveMatDamage;
    [SerializeField] private GameObject _PrefabSoul;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Transform target;
    [HideInInspector] public Transform targetProjectile;
    [HideInInspector] public Player_AFSFOR scriptPlayer;
    [HideInInspector] public bool isDetected = false;

    [HideInInspector] public SoundManager sM;
    [HideInInspector] public GameManager gM;

    private float scaleDuration = 1.0f;
    private bool isTakingDamage = false;
    private Renderer ghostRenderer;
    private float timerTakingDamage = -1;
    private float timerIsDetected = -1;
    public SoundType deathSoundType = SoundType.BeehGoat;

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
        sM = SuperManager.instance.soundManager;
        target = Player.instance.transform;
        scriptPlayer = target.gameObject.GetComponent<Player_AFSFOR>();
    }

    public void InitStatEnemy(int life, int damage, float speed)
    {
        _life = life;
        currentLife = _life;
        _damage = damage;
        currentLife = life;
        currentDamage = damage;
        _speed = speed;
        currentSpeed = _speed;
    }

    public void Get_Hit(int hit)
    {
        currentLife -= hit;
        SetIsTakingDamage(true);

        if (currentLife <= 0)
        {
            isDead = true;
            currentLife = 0;
            StartCoroutine(Die(false));
        }
    }

    public void Get_HitAll()
    {
        isDead = true;
        currentLife = 0;
        StartCoroutine(Die(true));
    }

    private IEnumerator Die(bool allDead)
    {
        // Joue le son de la mort --------------------------------------------------------
     
        sM.PlaySoundAtLocation(deathSoundType, 0.5f, this.transform.position);

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

        // Ajoute un kill au GameManager -----------------------------------------------
        if (gM != null)
        {
            if (!allDead)
            {
                gM.EnemyKilled();
                Instantiate(_PrefabSoul,transform.position, Quaternion.identity);
            }
        }

        scriptPlayer.AddMana(5);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Get_Hit(1000);
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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    #region SLAP DAT GOAT
    IEnumerator SlapEnemyAndRestoreRigidbody(float powerSlap)
    {
        rb.isKinematic = false;
        rb.AddForce((-1*this.gameObject.transform.forward + Vector3.up) * powerSlap*10);
        yield return new WaitForSeconds(2.0f);
        rb.isKinematic = true;
    }

    public void StartExpulsion(float powerslap)
    {
        StartCoroutine(SlapEnemyAndRestoreRigidbody(powerslap));
    }
    #endregion

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

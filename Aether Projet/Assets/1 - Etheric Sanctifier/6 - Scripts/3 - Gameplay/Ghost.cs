using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Global Behavior")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _timeOffsetToDisapear = 3.0f;
    [SerializeField] private float _maxDistanceFromPlayer = 2.5f;
    [SerializeField] private float _repulsiveForceNearPlayer = 30.0f;

    private float _timeToDisapear = -1;
    private float _timeToDamageMat = -1;

    [Header("Health")]
    [SerializeField] private float _health = 100.0f;

    [Header("Materials")]
    [SerializeField] private Material _baseMat;
    [SerializeField] private Material _emissiveMatDetection;
    [SerializeField] private Material _emissiveMatDamage;

    private MeshRenderer _ghostRenderer;

    [Header("RigidBody")]
    private Rigidbody _rbGhost;

    [Header("Index In List")]
    [SerializeField] private int _indexInManagerList;

    [Header("Is Detected")]
    [SerializeField] private bool _isDetected = false;
    [Header("Is Dead")]
    [SerializeField] private bool _isAlreadyDead = false;

    SoundManager sM;
    GhostManager gM;

    [Header("Attack")]
    [SerializeField] private float _attackTime = 2.0f;
    private bool _isAlreadyAttacking = false;
    private float _alarmEndAttack = -1;

    private void Start()
    {
        _ghostRenderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        _rbGhost = gameObject.GetComponent<Rigidbody>();

        gM = SuperManager.instance.ghostManager;
        sM = SuperManager.instance.soundManager;
    }

    private void Update()
    {
        if (_target != null)
        {
            MoveTowardPlayer();
            BlockMaximumApprochFromPlayer();
        }

        if (_timeToDisapear <= Time.time)
        {
            SetIsDetected(false);
        }

        if (_indexInManagerList < 0)
        {
            _indexInManagerList = 0;
            KillAndDestroyGhost();
        }
    }

    //  ###########################################
    //  ############  GHOST MOVEMENT  #############
    //  ###########################################
    private void MoveTowardPlayer()
    {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _moveSpeed);
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
    }
    private void BlockMaximumApprochFromPlayer()
    {
        if (Vector3.Distance(transform.position, _target.position) < _maxDistanceFromPlayer)
        {
            if (_alarmEndAttack < Time.time)
            {
                Attack();
            }
            AddForceToGhostOppositeToTarget(_repulsiveForceNearPlayer, ForceMode.Impulse);
        }
    }

    private void Attack()
    {
        _alarmEndAttack = Time.time + _attackTime;
        PlayAttackSoundAndVFXGhost();
    }

    //  ###########################################
    //  ###########  GHOST RIGIDBODY  #############
    //  ###########################################
    public void AddForceToGhost(Vector3 direction, float force, ForceMode forceMode)
    {
        _rbGhost.AddForce(direction.normalized * force, forceMode);
    }
    public void AddForceToGhostOppositeToTarget(float force, ForceMode forceMode)
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        _rbGhost.AddForce(-direction * force, forceMode);
    }

    //  ###########################################
    //  ##########  GHOST INITIALISATION  #########
    //  ###########################################
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    public void SetSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }


    //  ###########################################
    //  ########  GHOST HEALTH MANAGMENT  #########
    //  ###########################################
    public void LowerHealth(float damage)
    {
        _health = _health - damage;
        ChangeMaterialDamage(0.3f);

        if (_health <= 0)
        {
            if (!_isAlreadyDead)
            {
                KillAndDestroyGhost();
            }
        }
    }
    public void KillAndDestroyGhost()
    {
        _isAlreadyDead = true;
        PlayKillSoundAndVFXGhost();
        gM.GetCameraPlayer().GetComponent<Player>().AddMana();
        if (!gM.isTutorial)
        {
            SuperManager.instance.gameManagerAetherPunk.Set_KillGhost(false);
        }
        StartCoroutine(ScaleDownGhostAndDestroy());

    }
    private IEnumerator ScaleDownGhostAndDestroy()
    {
        float elapsedTime = 0f;
        float duration = 1.0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.time;
            yield return null;
        }
        transform.localScale = Vector3.zero;

        gM.RemoveGhostFromList(_indexInManagerList);
        Destroy(gameObject);
    }

    //  ###########################################
    //  ########  GHOST MATERIAL MANAGMENT  #######
    //  ###########################################
    public void SetIsDetected(bool newIsDetected)
    {
        _isDetected = newIsDetected;
        ChangeMaterial(_timeOffsetToDisapear);
    }
    public void ChangeMaterial(float time)
    {
        if (_isDetected)
        {
            _ghostRenderer.material = _emissiveMatDetection;
            _timeToDisapear = Time.time + time;
        }
        else
        {
            _ghostRenderer.material = _baseMat;
        }
    }
    public void ChangeMaterialDamage(float time)
    {
        _ghostRenderer.material = _emissiveMatDamage;
        _timeToDamageMat = Time.time + time;
    }

    //  ###########################################
    //  ##########  GHOST INDEX MANAGMENT  ########
    //  ###########################################
    public void SetIndexGhost(int newIndex)
    {
        _indexInManagerList = newIndex;
    }
    public void SetIndexMinusOne()
    {
        if (_indexInManagerList <= 0)
        {
            _indexInManagerList--;
        }
    }


    //  ###########################################
    //  ##########  GHOST SOUND AND FX  ###########
    //  ###########################################
    private void PlayAttackSoundAndVFXGhost()
    {
        SoundType s;
        if (gM.isTutorial)
        {
            s = SoundType.SlurpGoat;
        }
        else
        {
            s = SoundType.SlurpGoatReverb;
        }
        sM.PlaySoundAtLocation(s, 0.5f, this.transform.position);
        SuperManager.instance.vfxManager.InstantiateVFX_vfxDeadGhostImpact(this.transform);
    }
    private void PlayKillSoundAndVFXGhost()
    {
        SoundType s;
        if (gM.isTutorial)
        {
            s = SoundType.BeehGoat;
        }
        else
        {
            s = SoundType.BeehGoatReverb;
        }
        sM.PlaySoundAtLocation(s, 0.5f, this.transform.position);
        SuperManager.instance.vfxManager.InstantiateVFX_vfxDeadGhostImpact(this.transform);
    }
}

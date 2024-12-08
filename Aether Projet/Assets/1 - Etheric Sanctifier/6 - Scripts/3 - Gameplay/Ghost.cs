using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Global Behavior")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _timeOffsetToDisapear = 5.0f;
    [SerializeField] private float _timeToDisapear = -1;

    [Header("Health")]
    [SerializeField] private float _health = 100.0f;

    [Header("Materials")]
    [SerializeField] private Material _baseMat;
    [SerializeField] private Material _emissiveMat;
    [SerializeField] private bool _isDetected = false;
    [SerializeField] private MeshRenderer _ghostRenderer;

    private void Start()
    {
        _ghostRenderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = _target.position - transform.position;
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _moveSpeed);
            }


            transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        if(_timeToDisapear<=Time.time)
        {
            SetIsDetected(false);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    public void SetSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }
    public void LowerHealth(float damage)
    {
        _health = _health - damage;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetIsDetected(bool newIsDetected)
    {
        _isDetected = newIsDetected;
        ChangeMaterial();
    }
    public void ChangeMaterial()
    {
        if (_isDetected)
        {
            _ghostRenderer.material = _emissiveMat;
            _timeToDisapear = Time.time + _timeOffsetToDisapear;
        }
        else
        {
            _ghostRenderer.material = _baseMat;
        }
    }
}

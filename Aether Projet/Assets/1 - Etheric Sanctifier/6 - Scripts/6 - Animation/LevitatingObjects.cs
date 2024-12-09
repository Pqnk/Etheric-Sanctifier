using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitatingObjects : MonoBehaviour
{
    [SerializeField] private float levitationRange = 1.0f;
    [SerializeField] private float levitationSpeed = 2.0f;
    [SerializeField] private float rotationAngle = 45.0f;
    [SerializeField] private float rotationSpeed = 1.5f;

    [SerializeField] private bool _isLevitating = true;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        timeOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        if (_isLevitating)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * levitationSpeed + timeOffset) * levitationRange;
            float newRotationAngle = Mathf.Sin(Time.time * rotationSpeed + timeOffset) * rotationAngle;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            transform.rotation = startRotation * Quaternion.Euler(0f, newRotationAngle, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isLevitating = false;
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}

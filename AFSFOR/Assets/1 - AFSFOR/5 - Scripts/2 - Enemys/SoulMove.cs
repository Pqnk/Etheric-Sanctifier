using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulMove : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    AstroBuster target;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<AstroBuster>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(MoveTarget());
    }

    IEnumerator MoveTarget()
    {
        while (Vector3.Distance(transform.position, target.transform.position) > 0.01f)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        //target.

        Destroy(gameObject);
    }
}

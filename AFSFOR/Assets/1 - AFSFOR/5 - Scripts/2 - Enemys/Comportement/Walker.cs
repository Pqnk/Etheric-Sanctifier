using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class Walker : MonoBehaviour
{
    private Transform target;
    private Enemy enemy;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        // Temporaire mais donner au script enemy à l'instantiation
        target = GameObject.Find("Player").transform;

        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        enemy.rb.MovePosition(transform.position + direction * enemy.currentSpeed * Time.fixedDeltaTime);
        transform.forward = direction;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10f);
        }
    }

}

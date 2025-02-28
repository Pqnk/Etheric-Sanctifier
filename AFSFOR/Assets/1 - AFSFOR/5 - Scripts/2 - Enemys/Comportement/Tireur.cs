using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Tireur : MonoBehaviour
{
    private Enemy enemy;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;

    private float randomMoveSpeed = 1f;
    private float randomMoveAmount = 0.5f;
    private float nextFireTime;
    private Vector3 randomOffset;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        transform.LookAt(enemy.target.position);

        MoveTowardsTarget();
        RandomMovement();
        ShootAtTarget();
    }

    void MoveTowardsTarget()
    {
        if (enemy.target == null) return;

        float distance = Vector3.Distance(transform.position, enemy.target.position);
        Vector3 direction = (enemy.target.position - transform.position).normalized;

        if (distance > enemy.stopDistance + 1)
        {
            transform.position += direction * enemy.currentSpeed * Time.deltaTime;
        }
        else if (distance < enemy.stopDistance - 1)
        {
            transform.position -= direction * enemy.currentSpeed * Time.deltaTime;
        }
    }

    void RandomMovement()
    {
        float randomX = Mathf.Sin(Time.time * randomMoveSpeed) * randomMoveAmount;
        float randomY = Mathf.Cos(Time.time * randomMoveSpeed * 0.8f) * randomMoveAmount;

        transform.position += new Vector3(randomX, randomY, 0) * Time.deltaTime;
    }

    void ShootAtTarget()
    {
        if (enemy.target == null || Time.time < nextFireTime) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (enemy.target.position - firePoint.position).normalized;
            rb.velocity = direction * 10f;
        }

        nextFireTime = Time.time + fireRate;
    }
}

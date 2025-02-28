using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class HeadBoss : MonoBehaviour
{
    private Enemy enemy;

    [Header("Info Tir")]
    [SerializeField] float shootCooldown = 2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform[] spawnPoints;

    [Header("Info Spawn Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Info charge")]
    [SerializeField] float chargeDuration = 2f;
    [SerializeField] float repliDuration = 2f;
    [SerializeField] int vieRetirerAvantRepli = 2;

    [Header("Info Tir")]
    [SerializeField] float timerBeforePatern = 5f;

    [Header("Info Move")]
    [SerializeField] float hoverAmplitude = 0.2f;
    [SerializeField] float hoverSpeed = 3f;
    [SerializeField] private float randomDirectionChangeTime = 3f; // Temps entre chaque changement de direction
    private float hoverTime = 0f;
    private float directionChangeTimer = 0f;
    private int rotationDirection = 1;

    private bool isCharging = false;
    private bool canMove = true;
    private bool isAttacking = false;

    private int startingHealth;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        StartCoroutine(BossPatternRoutine());
    }

    void Update()
    {
        if (enemy.target != null && canMove && !isAttacking)
        {
            Move();
        }
    }

    #region Move
    private void Move()
    {
        Vector3 direction = (enemy.target.position - transform.position).normalized;
        direction.y = 0;

        float distance = direction.magnitude;

        if (distance > enemy.stopDistance + 1f)
        {
            transform.position += direction * enemy.currentSpeed * Time.deltaTime;
        }
        else if (distance < enemy.stopDistance - 1f)
        {
            transform.position -= direction * enemy.currentSpeed * Time.deltaTime;
        }

        // mouvement haut bas
        hoverTime += Time.deltaTime;
        float hoverOffset = Mathf.Sin(hoverTime * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, 5f + hoverOffset, transform.position.z); // 5f = hauteur de base

        // direction random
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= randomDirectionChangeTime)
        {
            directionChangeTimer = 0f;
            rotationDirection = (UnityEngine.Random.Range(0, 2) == 0) ? 1 : -1;
        }

        transform.RotateAround(enemy.target.position, Vector3.up, rotationDirection * enemy.rotationSpeed);
        transform.LookAt(new Vector3(enemy.target.position.x, transform.position.y, enemy.target.position.z));
    }
    #endregion

    IEnumerator BossPatternRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timerBeforePatern);

            int randomPattern = UnityEngine.Random.Range(0, 3);

            if (randomPattern == 0)
            {
                yield return StartCoroutine(SpawnEnemies());
            }
            else if (randomPattern == 1)
            {
                yield return StartCoroutine(ChargeAttack());
            }
            else if (randomPattern == 2)
            {
                yield return StartCoroutine(ShootingPattern());
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        isAttacking = true;
        canMove = false;
        yield return new WaitForSeconds(1f);

        foreach (Transform spawn in spawnPoints)
        {
            Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
    }

    #region Charge Attack
    IEnumerator ChargeAttack()
    {
        isAttacking = true;
        canMove = false;
        isCharging = true;

        startingHealth = enemy.currentLife;

        yield return StartCoroutine(ShakeEffect(0.5f, 0.2f));

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = enemy.target.position;
        float timer = 0f;

        while (timer < chargeDuration && isCharging)
        {
            timer += Time.deltaTime;
            float progress = timer / chargeDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            if (Vector3.Distance(transform.position, enemy.target.position) <= 1.5f)
            {
                enemy.scriptPlayer.DamagerPlayer(enemy.currentDamage);
                break;
            }

            if (startingHealth - enemy.currentLife >= vieRetirerAvantRepli)
            {
                isCharging = false;
                break;
            }

            yield return null;
        }

        isCharging = false;

        yield return StartCoroutine(RepositionAfterCharge());
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        canMove = true;
    }

    IEnumerator RepositionAfterCharge()
    {
        Vector3 retreatDirection = (transform.position - enemy.target.position).normalized;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = enemy.target.position + retreatDirection * enemy.stopDistance;
        targetPosition.y = 5f;

        float retreatTimer = 0f;

        while (retreatTimer < repliDuration)
        {
            retreatTimer += Time.deltaTime;
            float progress = retreatTimer / repliDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }

        transform.position = targetPosition;
    }

    IEnumerator ShakeEffect(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * magnitude;
            randomOffset.y = 0; // Empêcher les vibrations en hauteur
            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
    #endregion

    IEnumerator ShootingPattern()
    {
        isAttacking = true;
        canMove = false;

        yield return new WaitForSeconds(1f);

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        yield return new WaitForSeconds(shootCooldown);
        isAttacking = false;
        canMove = true;
    }
}

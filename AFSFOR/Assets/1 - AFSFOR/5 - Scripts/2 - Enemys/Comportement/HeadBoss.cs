using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class HeadBoss : MonoBehaviour
{
    private Enemy enemy;

    [Header("Info Tir")]
    [SerializeField] float shootCooldown = 2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileHeavyPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform UIPoint;
    [SerializeField] Transform[] spawnPoints;

    [Header("Info Spawn Enemy")]
    [SerializeField] GameObject[] enemyPrefab;

    [Header("Info charge")]
    [SerializeField] float chargeDuration = 2f;
    [SerializeField] float repliDuration = 2f;
    [SerializeField] int vieRetirerAvantRepli = 2;

    [Header("Info Tir")]
    [SerializeField] float timerBeforePatern = 5f;

    [Header("Info Move")]
    [SerializeField] float hoverAmplitude = 0.2f;
    [SerializeField] float hoverSpeed = 3f;
    [SerializeField] private float randomDirectionChangeTime = 3f;
    private float hoverTime = 0f;
    private float directionChangeTimer = 0f;
    private int rotationDirection = 1;

    private bool isCharging = false;
    private bool canMove = true;
    private bool isAttacking = false;

    private int startingHealth;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        StartCoroutine(BossPatternRoutine());
        PointsLookAtPlayer();
        enemy.InitStatEnemy(SuperManager.instance.damageManager.lifeBOSS, SuperManager.instance.damageManager.damageBOSS, SuperManager.instance.damageManager.speedBOSS);
        enemy.rotationSpeed = SuperManager.instance.damageManager.rotationSpeedBOSS;
        UIPoint.GetComponentInChildren<Slider>().maxValue = SuperManager.instance.damageManager.lifeBOSS;
    }

    void Update()
    {
        UIPoint.GetComponentInChildren<Slider>().value = enemy.currentLife;

        if (enemy.target != null && canMove && !isAttacking && !enemy.idDead)
        {
            Move();
        }
    }

    private void PointsLookAtPlayer()
    {
        firePoint.GetComponent<LookAtPlayer>().target = enemy.target;
        UIPoint.GetComponent<LookAtPlayer>().target = enemy.target;

        foreach (var item in spawnPoints)
        {
            item.GetComponent<LookAtPlayer>().target = enemy.target;
        }
    }

    IEnumerator BossPatternRoutine()
    {
        while (!enemy.idDead)
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

    #region Move
    private void Move()
    {
        Vector3 direction = (enemy.target.position - transform.position).normalized;
        direction.y = 0;

        //float distance = direction.magnitude;
        float distance = Vector3.Distance(transform.position, enemy.target.position);

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

    #region Spawn Ennemies
    IEnumerator SpawnEnemies()
    {
        isAttacking = true;
        canMove = false;
        yield return new WaitForSeconds(1f);

        int chance = UnityEngine.Random.Range(0, 3);

        switch (chance)
        {
            case 0:
                foreach (Transform spawn in spawnPoints)
                {
                    if (spawn.gameObject.name.Contains("2") || spawn.gameObject.name.Contains("3"))
                    {
                        Instantiate(enemyPrefab[0], spawn.position, Quaternion.identity);
                    }
                }
                break;

            case 1:
                foreach (Transform spawn in spawnPoints)
                {
                    if (spawn.gameObject.name.Contains("1") || spawn.gameObject.name.Contains("4") || spawn.gameObject.name.Contains("5"))
                    {
                        Instantiate(enemyPrefab[1], spawn.position, Quaternion.identity);
                    }
                }
                break;

            case 2:
                foreach (Transform spawn in spawnPoints)
                {
                    if (spawn.gameObject.name.Contains("1"))
                    {
                        Instantiate(enemyPrefab[2], spawn.position, Quaternion.identity);
                    }
                }
                break;
        }

        yield return new WaitForSeconds(2f);
        isAttacking = false;
        canMove = true;
    }
    #endregion

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
            randomOffset.y = 0;
            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
    #endregion

    #region Tir
    IEnumerator ShootingPattern()
    {
        isAttacking = true;
        canMove = false;

        yield return new WaitForSeconds(1f);

        int chance = UnityEngine.Random.Range(0, 2);

        if (chance == 0)
        {
            GameObject projectile =  Instantiate(projectileHeavyPrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<EnemyBullet>().targetEnemyBullet = enemy.targetProjectile;
        }
        else
        {
            foreach (Transform spawn in spawnPoints)
            {
                GameObject projectile = Instantiate(projectilePrefab, spawn.position, spawn.rotation);
                projectile.GetComponent<EnemyBullet>().targetEnemyBullet = enemy.targetProjectile;
                yield return new WaitForSeconds(.5f);
            }
        }

        yield return new WaitForSeconds(shootCooldown);
        isAttacking = false;
        canMove = true;
    }
    #endregion
}

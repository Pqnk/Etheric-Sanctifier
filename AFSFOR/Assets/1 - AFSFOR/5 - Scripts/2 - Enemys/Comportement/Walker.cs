using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class Walker : MonoBehaviour
{
    private Enemy enemy;

    [Header("Repli")]
    public float repliDistance = 2f;
    public float repliDuration = 0.5f;
    private bool isRepli = false;
    private float repliTimer = 0f;

    [Header("Esquive")] 
    public float esquiveDistance = 3f;
    public float esquiveDuration = 0.3f;
    public float esquiveDetectionRadius = 5f;
    public float esquiveChance = 50f;
    private bool isEsquive = false;
    private bool checkedEsquive = false;
    private Vector3 esquiveDirection;
    private float esquiveTimer = 0f;
    private LayerMask projectileLayer;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        projectileLayer = LayerMask.GetMask("Bullet");
    }

    private void FixedUpdate()
    {
        if (isEsquive)
        {
            PerformEsquive();
            return;
        }
                
        if (DetectProjectile())
        {
            if (!checkedEsquive)
            {
                checkedEsquive = true;
                if (CheckSiDoitEsquive())
                {
                    StartEsquive();
                    return;
                }
            }
        }
        else
        {
            checkedEsquive = false;
        }

        if (enemy.target != null)
        {
            Move();
        }

    }

    void Move()
    {
        Vector3 direction = (enemy.target.position - transform.position).normalized;
        direction.y = 0;

        if (isRepli)
        {
            Retreat(direction);
            return;
        }

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10f);
        }

        if (Vector3.Distance(transform.position, enemy.target.position) > enemy.stopDistance)
        {
            enemy.rb.MovePosition(transform.position + direction * enemy.currentSpeed * Time.fixedDeltaTime);
            transform.forward = direction;
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Attaque");
        enemy.target.gameObject.GetComponent<Player_AFSFOR>().DamagerPlayer(enemy.currentDamage);

        // Joue le son
        SoundType s;
        s = SoundType.SlurpGoat;
        enemy.sM.PlaySoundAtLocation(s, 0.5f, this.transform.position);

        // Repli
        isRepli = true;
        repliTimer = repliDuration;
    }

    #region Recul
    void Retreat(Vector3 direction)
    {
        if (repliTimer > 0)
        {
            repliTimer -= Time.deltaTime;
            Vector3 retreatDirection = -direction.normalized;
            enemy.rb.MovePosition(transform.position + retreatDirection * (repliDistance / repliDuration) * Time.deltaTime);
        }
        else
        {
            isRepli = false;
        }
    }
    #endregion

    #region Esquive
    bool DetectProjectile()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, esquiveDetectionRadius, projectileLayer);
        return hits.Length > 0;
    }
    bool CheckSiDoitEsquive()
    {
        return UnityEngine.Random.value * 100f < esquiveChance;
    }

    void StartEsquive()
    {
        isEsquive = true;
        esquiveTimer = esquiveDuration;
        esquiveDirection = transform.right * (UnityEngine.Random.value > 0.5f ? 1 : -1);
    }

    void PerformEsquive()
    {
        if (esquiveTimer > 0)
        {
            // Son esquive
            esquiveTimer -= Time.deltaTime;
            enemy.rb.MovePosition(transform.position + esquiveDirection * (esquiveDistance / esquiveDuration) * Time.deltaTime);
        }
        else
        {
            isEsquive = false;
        }
    }
    #endregion
}

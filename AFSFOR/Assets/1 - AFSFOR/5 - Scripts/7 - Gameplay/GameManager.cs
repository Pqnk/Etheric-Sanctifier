using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int enemyToKill;
        public float spawnDelay;
    }

    [Header("Info Wave")]
    [SerializeField] Wave[] waves;
    [SerializeField] GameObject[] bossPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float waveInterval = 10f;

    int idBoss = 0;
    int totalEnemiesKilled = 0;
    int currentWaveIndex = 0;
    int enemiesKilledThisWave = 0;
    int enemiesToKillThisWave = 0;
    bool waveInProgress = false;
    bool bossWaveActive = false;
    Transform player;


    void Start()
    {
        player = Player.instance.transform;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < 2; i++) // Ennemi Vague
            {
                yield return StartCoroutine(SpawnWaves(waves[currentWaveIndex % waves.Length]));
                yield return new WaitUntil(() => enemiesKilledThisWave >= enemiesToKillThisWave);
                KillAllEnemies();
                yield return new WaitForSeconds(waveInterval);
            }

            // Boss Vague
            yield return StartCoroutine(SpawnBossWaves());
            yield return new WaitUntil(() => !bossWaveActive);
            KillAllEnemies();
            yield return new WaitForSeconds(waveInterval);
            currentWaveIndex++;
        }
    }

    IEnumerator SpawnWaves(Wave wave)
    {
        waveInProgress = true;
        enemiesKilledThisWave = 0;
        enemiesToKillThisWave = wave.enemyToKill;

        while (enemiesKilledThisWave < enemiesToKillThisWave)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().target = player;
            yield return new WaitForSeconds(wave.spawnDelay);
        }
    }

    IEnumerator SpawnBossWaves()
    {
        waveInProgress = true;
        enemiesKilledThisWave = 0;
        enemiesToKillThisWave = 1;
        bossWaveActive = true;

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject boss = Instantiate(bossPrefab[idBoss], spawnPoint.position, Quaternion.identity);
        boss.GetComponent<Enemy>().target = player;

        if (idBoss == 0)
        {
            idBoss = 1;
        }
        else
        {
            idBoss = 0;
        }

        yield return null;
    }

    public void EnemyKilled()
    {
        totalEnemiesKilled++;
        enemiesKilledThisWave++;
        Debug.Log("Enemies killed: " + totalEnemiesKilled);

        if (bossWaveActive && enemiesKilledThisWave >= enemiesToKillThisWave)
        {
            bossWaveActive = false;
        }
    }

    private void KillAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Get_Hit(1000000);
        }
    }
}

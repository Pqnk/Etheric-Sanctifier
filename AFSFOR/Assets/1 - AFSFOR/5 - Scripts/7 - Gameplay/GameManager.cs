using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Player.instance.gameObject.GetComponent<Player_AFSFOR>().ToggleHeadLight(true);
        player = Player.instance.transform;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            if((currentWaveIndex + 1) % 3 == 0)
            {
                yield return StartCoroutine(SpawnBossWaves());
                yield return new WaitUntil(() => !bossWaveActive);                
            }
            else
            {
                yield return StartCoroutine(SpawnWaves(waves[currentWaveIndex % waves.Length]));
                yield return new WaitUntil(() => enemiesKilledThisWave >= enemiesToKillThisWave);
            }

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
            enemy.GetComponent<Enemy>().gM = this;
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
        boss.GetComponent<Enemy>().gM = this;

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
            enemy.Get_HitAll();
        }
    }
}

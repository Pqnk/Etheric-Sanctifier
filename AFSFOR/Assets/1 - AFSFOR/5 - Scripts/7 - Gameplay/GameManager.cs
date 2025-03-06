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

    [Space]
    /*[HideInInspector]*/
    public int enemiesKilledThisWave = 0;
    /*[HideInInspector]*/
    public int enemiesToKillThisWave = 0;
    /*[HideInInspector]*/
    public int currentWaveIndex = 0;
    /*[HideInInspector]*/
    public int currentWaveIndexGlobal = 1;

    int idBoss = 0;
    int totalEnemiesKilled = 0;
    public bool waveInProgress = false;
    public bool bossWaveActive = false;
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
        yield return new WaitForSeconds(waveInterval);

        while (true)
        {
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Horn, 0.8f, player.transform.position);

            if ((currentWaveIndexGlobal) % 3 == 0)
            {
                yield return StartCoroutine(SpawnBossWaves());
                yield return new WaitUntil(() => !bossWaveActive);
            }
            else
            {
                yield return StartCoroutine(SpawnWaves(waves[currentWaveIndex % waves.Length]));
                yield return new WaitUntil(() => enemiesKilledThisWave >= enemiesToKillThisWave);
                currentWaveIndex++;
            }

            KillAllEnemies();
            yield return new WaitForSeconds(waveInterval);
            currentWaveIndexGlobal++;
            Player.instance.gameObject.GetComponent<Player_AFSFOR>().alreadySetMaxScore = false;
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

        Transform[] newWaypoints = { spawnPoints[1], spawnPoints[2], spawnPoints[3], spawnPoints[8], spawnPoints[9], spawnPoints[10] };
        Transform spawnPoint = newWaypoints[UnityEngine.Random.Range(0, newWaypoints.Length)];
        GameObject boss = Instantiate(bossPrefab[idBoss], spawnPoint.position, Quaternion.identity);
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

        if (bossWaveActive)
        {
            bossWaveActive = false;
        }
    }

    private void KillAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        if (enemies.Length == 0) return;

        foreach (Enemy enemy in enemies)
        {
            enemy.Get_HitAll();
        }
    }
}

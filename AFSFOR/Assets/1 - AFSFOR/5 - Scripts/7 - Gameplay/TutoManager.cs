using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TutoManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] spawnPoints;

    Transform player;


    void Start()
    {
        Player.instance.gameObject.GetComponent<Player_AFSFOR>().ToggleHeadLight(true);
        player = Player.instance.transform;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(5);

        while (true)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().targetProjectile = Player.instance.GetComponent<Player_AFSFOR>().VRHead.transform;
            yield return new WaitForSeconds(5);
        }
    }
}

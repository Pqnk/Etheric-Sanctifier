using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostsSpawnPoints : MonoBehaviour
{
    [Header("Ghost Prefab")]
    [SerializeField] private GameObject ghostPrefab;
    private Transform[] _spawnPoints;

    [Header("Spawn Settings")]
    private float _moveSpeed;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _moveSpeedMin = 2f;
    [SerializeField] private float _moveSpeedMax = 100f;

    private void Start()
    {
        _spawnPoints = GetAllChildren();
        StartCoroutine(SpawnPrefabs());
    }

    public Transform[] GetAllChildren()
    {
        int childCount = transform.childCount;
        Transform[] children = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject.transform;
        }

        return children;
    }

    IEnumerator SpawnPrefabs()
    {
        while (true)
        {
            Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Vector3 spawnPosition = randomSpawnPoint.position;
            GameObject ghostInstance = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);

            Ghost ghostBehavior = ghostInstance.GetComponent<Ghost>();
            ghostBehavior.SetTarget(this.gameObject.transform);
            _moveSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
            ghostBehavior.SetSpeed(_moveSpeed);

            yield return new WaitForSeconds(_spawnInterval);
        }
    }



}

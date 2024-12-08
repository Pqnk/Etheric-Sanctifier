using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [Header("Ghost Prefab")]
    [SerializeField] private GameObject _ghostPrefab;

    [Header("Spawn Points Parent")]
    [SerializeField] private GameObject _spawnPointsParent;
    private Transform[] _spawnPoints;

    [Header("Spawn Settings")]
    private float _moveSpeed;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _moveSpeedMin = 2f;
    [SerializeField] private float _moveSpeedMax = 100f;
    [SerializeField] private bool _canSpawn = false;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _target;

    private void Start()
    {
        _spawnPoints = GetAllChildren();
    }

    public Transform[] GetAllChildren()
    {
        int childCount = _spawnPointsParent.transform.childCount;
        Transform[] children = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = _spawnPointsParent.transform.GetChild(i).gameObject.transform;
        }

        return children;
    }

    IEnumerator SpawnPrefabs()
    {
        while (true)
        {
            Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Vector3 spawnPosition = randomSpawnPoint.position;
            GameObject ghostInstance = Instantiate(_ghostPrefab, spawnPosition, Quaternion.identity);

            Ghost ghostBehavior = ghostInstance.GetComponent<Ghost>();
            ghostBehavior.SetTarget(_target);
            _moveSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
            ghostBehavior.SetSpeed(_moveSpeed);

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public void SetCanSpawn(bool newCanSpawn)
    {
        _canSpawn = newCanSpawn;

        if (_canSpawn)
        {
            StartCoroutine(SpawnPrefabs());
        }
        else
        {
            StopCoroutine(SpawnPrefabs());
        }
    }
}

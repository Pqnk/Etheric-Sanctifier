using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostManager : MonoBehaviour
{
    [Header("Goat Prefab")]
    [SerializeField] private GameObject _goatPrefab;

    [Header("Ghost Prefab")]
    [SerializeField] private GameObject _ghostPrefab;

    [Header("Spawn Settings : Main Target")]
    [SerializeField] private string _mainTargetName = "Camera_Player";
    [SerializeField] private GameObject _cameraPlayer;
    [SerializeField] private Transform _mainTarget;
    [SerializeField] private Transform _secondaryTarget;
    [SerializeField] private GameObject _radarRef;

    [Header("Spawn Points Parent")]
    [SerializeField] private string _ghostSpawnPointsName = "--GHOST SPAWN POINTS--";
    [SerializeField] private GameObject _spawnPointsParent;
    private Transform[] _spawnPoints;

    [Header("Spawn Settings")]
    private float _moveSpeed;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _moveSpeedMin = 2f;
    [SerializeField] private float _moveSpeedMax = 100f;
    [SerializeField] private bool _canSpawn = false;


    [Header("List of All ghost in scene")]
    public List<Transform> allGhosts;

    [Header("Max ghost in the scene")]
    [SerializeField] private int _maxGhostInTotal = 10;

    [Header("Max ghost in the scene")]
    [SerializeField] private bool _isReady = false;

    [Header("Time Between Wave")]
    public float timeBetweenWave = 15.0f;

    [Header("Is it a Tutorial ?")]
    public bool isTutorial = false;

    private void Start()
    {

    }

    public Transform GetMainTarget()
    {
        return _mainTarget;
    }

    public GameObject GetCameraPlayer()
    {
        return _cameraPlayer;
    }

    public bool InitializeghostManager(bool isTuto)
    {
        LevelType levelType = LevelType.HUB;

        isTutorial = isTuto;

        if (isTutorial)
        {
            levelType = LevelType.Tutorial;
        }
        else
        {
            levelType = LevelType.Level01;

        }
        _spawnPointsParent = SuperManager.instance.levelManager.FindInScene(levelType, _ghostSpawnPointsName);
        _cameraPlayer = SuperManager.instance.levelManager.FindInScene(levelType, _mainTargetName);
        if (_cameraPlayer == null)
        {
            Debug.LogError("PAS DE CAMERA MAIN TARGET");
        }
        else
        {
            _mainTarget = _cameraPlayer.transform.GetChild(2).transform;
            _radarRef = _cameraPlayer.transform.GetChild(2).transform.GetChild(0).gameObject;
        }

        if (_spawnPointsParent != null && _mainTarget != null)
        {
            _isReady = true;
            _spawnPoints = GetAllChildren();
            return true;
        }
        else
        {
            _isReady = false;
            return false;
        }
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
        while (allGhosts.Count < _maxGhostInTotal)
        {
            Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Vector3 spawnPosition = randomSpawnPoint.position;
            GameObject p;
            if (isTutorial)
            {
                p = _goatPrefab;
            }
            else
            {
                p = _ghostPrefab;
            }
            GameObject ghostInstance = Instantiate(p, spawnPosition, Quaternion.identity);

            Ghost ghostBehavior = ghostInstance.GetComponent<Ghost>();
            ghostBehavior.SetTarget(_mainTarget);
            _moveSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
            ghostBehavior.SetSpeed(_moveSpeed);

            allGhosts.Add(ghostBehavior.transform);
            ghostBehavior.SetIndexGhost(allGhosts.Count - 1);

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public void SetCanSpawn(bool newCanSpawn)
    {
        _canSpawn = newCanSpawn;
        _radarRef.GetComponent<Radar>().ToggleRadar(_canSpawn);

        if (_canSpawn)
        {
            StartCoroutine(SpawnPrefabs());
        }
        else
        {
            StopCoroutine(SpawnPrefabs());
        }
    }

    public void RemoveGhostFromList(int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        allGhosts.RemoveAt(index);

        foreach (Transform t in allGhosts)
        {
            t.gameObject.GetComponent<Ghost>().SetIndexMinusOne();
        }
    }

    public void StopWaveAndClearAllGhost()
    {
        SetCanSpawn(false);

        foreach (Transform t in allGhosts)
        {
            Destroy(t.gameObject);
        }

        allGhosts.Clear();

        StartCoroutine(NextWave());
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        SetCanSpawn(true);
    }


    public void DefinitiveStopWaveAndClearGhosts()
    {
        SetCanSpawn(false);

        foreach (Transform t in allGhosts)
        {
            Destroy(t.gameObject);
        }

        allGhosts.Clear();
    }
}

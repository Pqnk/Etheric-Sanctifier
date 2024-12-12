using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] private Transform _lowTarget;
    [SerializeField] private GameObject _radarRef;
    private List<Transform> _targets;

    [Header("Spawn Points Parent")]
    [SerializeField] private string _ghostSpawnPointsName = "--GHOST SPAWN POINTS--";
    [SerializeField] private GameObject _spawnPointsParent;
    private Transform[] _spawnPoints;

    [Header("Spawn Settings")]
    private float _moveSpeed;
    [SerializeField] private float[] _spawnInterval;
    [SerializeField] private float _moveSpeedMin = 2f;
    [SerializeField] private float _moveSpeedMax = 100f;
    [SerializeField] private bool _canSpawn = false;

    [Header("Taille des ghost")]
    public float tailleGhostMin = .7f;
    public float tailleGhostMax = 1.2f;

    [Header("Material par défaut ghost")]
    public Material[] baseMats;

    [Header("List of All ghost in scene")]
    public List<Transform> allGhosts;

    [Header("Max ghost in the scene")]
    [SerializeField] private int _maxGhostInTotal = 10;

    [SerializeField] private bool _isReady = false;

    [Header("Time Between Wave")]
    public float timeBetweenWave = 15.0f;

    [Header("Is it a Tutorial ?")]
    public bool isTutorial = false;

    public Transform GetMainTarget()
    {
        return _mainTarget;
    }
    public GameObject GetCameraPlayer()
    {
        return _cameraPlayer;
    }

    public Transform GetTargetLow()
    {
        return _lowTarget;
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
            _lowTarget = _cameraPlayer.transform.GetChild(2).transform.GetChild(2).transform;
            //_targets.Add(_mainTarget);
            //_targets.Add(_lowTarget);
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
        float spawnIntervalActual = 5f;

        if (allGhosts.Count < _maxGhostInTotal && _canSpawn)
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

            // Changement de la size
            float rd = Random.Range(tailleGhostMin, tailleGhostMax);
            ghostInstance.transform.localScale = new Vector3(rd, rd, rd);

            Ghost ghostBehavior = ghostInstance.GetComponent<Ghost>();

            //ghostBehavior.SetTarget(_mainTarget);
            ghostBehavior.SetTarget(GetRandomTargetBetweenMainAndLow());
            // Vitesse
            _moveSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
            ghostBehavior.SetSpeed(_moveSpeed);


            switch (SuperManager.instance.gameManagerAetherPunk.indexPalier)
            {
                case 0:
                    // Life
                    ghostBehavior.Set_Life(100);
                    spawnIntervalActual = _spawnInterval[0];
                    ghostBehavior.Set_BaseMat(baseMats[0]);
                    break;

                case 1:
                    // Life
                    ghostBehavior.Set_Life(200);
                    spawnIntervalActual = _spawnInterval[1];
                    ghostBehavior.Set_BaseMat(baseMats[1]);
                    break;

                case 2:
                    // Life
                    ghostBehavior.Set_Life(300);
                    spawnIntervalActual = _spawnInterval[2];
                    ghostBehavior.Set_BaseMat(baseMats[2]);
                    break;
            }

            allGhosts.Add(ghostBehavior.transform);
            ghostBehavior.SetIndexGhost(allGhosts.Count - 1);

        }

        yield return new WaitForSeconds(spawnIntervalActual);
        StartCoroutine(SpawnPrefabs());
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
            t.gameObject.GetComponent<Ghost>().KillAndDestroyGhost();
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
            t.gameObject.GetComponent<Ghost>().KillAndDestroyGhost();
        }

        allGhosts.Clear();
    }

    private Transform GetRandomTargetBetweenMainAndLow()
    {
        int randomIndex = Random.Range(0, 1);

        if (randomIndex == 0)
        {
            return _mainTarget;
        }
        else
        {
            return _lowTarget;
        }
    }

    public void UpdateMaxGhost(int indexPalier)
    {
        switch (indexPalier)
        {
            case 0:
                _maxGhostInTotal = SuperManager.instance.gameManagerAetherPunk.palierKills[0];
                break;

            case 1:
                _maxGhostInTotal = SuperManager.instance.gameManagerAetherPunk.palierKills[1];
                break;

            case 2:
                _maxGhostInTotal = SuperManager.instance.gameManagerAetherPunk.palierKills[2];
                break;
        }
    }
}

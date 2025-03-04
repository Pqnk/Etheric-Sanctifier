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

    [Header("Walker Prefab")]
    [SerializeField] private GameObject _walkerPrefab;

    [Header("Spawn Settings : Main Target")]
    [SerializeField] private string _mainTargetName = "Player_AFSFOR";
    [SerializeField] private GameObject _playerRef;
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

    [Header("Parametre Ghost")]
    public float tailleGhostMin = .7f;
    public float tailleGhostMax = 1.2f;
    [Tooltip("Liste des materiaux de base des ghost")]
    public Material[] baseMats;
    [Tooltip("Liste des points de vie par palier des ghost")]
    public float[] lifesPalier;

    [Header("List of All ghost in scene")]
    public List<Transform> allGhosts;

    [Header("Max ghost in the scene")]
    [SerializeField] private int _maxGhostInTotal = 10;

    [SerializeField] private bool _isReady = false;

    [Header("Time Between Wave")]
    public float timeBetweenWave = 15.0f;

    [Header("Is it a Tutorial ?")]
    public bool isTutorial = false;

    private int _currentIdGhost = 0;

    public Transform GetMainTarget()
    {
        return _mainTarget;
    }
    public GameObject GetCameraPlayer()
    {
        return _playerRef;
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
        //_playerRef = SuperManager.instance.levelManager.FindInScene(levelType, _mainTargetName);
        if (_playerRef == null)
        {
            Debug.LogError("PAS DE PLAYER_AFSFOR DANS LA SCENE");
        }
        else
        {
            //_mainTarget =   _playerRef.transform.GetChild(6).transform;
            //_lowTarget =    _playerRef.transform.GetChild(6).transform.GetChild(0).transform;
            //_radarRef =     _playerRef.transform.GetChild(6).transform.GetChild(1).gameObject;
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
                p = _walkerPrefab;
            }
            else
            {
                p = _ghostPrefab;
            }
            GameObject ghostInstance = Instantiate(p, spawnPosition, Quaternion.identity);
            ghostInstance.GetComponent<Enemy>().SetTarget(_mainTarget);

            SuperManager.instance.vfxManager.InstantiateVFX_vfxDeadGhostImpact(ghostInstance.transform);

            // Changement de la size
            //float rd = Random.Range(tailleGhostMin, tailleGhostMax);
            //ghostInstance.transform.localScale = new Vector3(rd, rd, rd);

            //Ghost ghostBehavior = ghostInstance.GetComponent<Ghost>();

            //ghostBehavior.SetTarget(GetRandomTargetBetweenMainAndLow());
            //ghostBehavior.SetId(_currentIdGhost);
            //_currentIdGhost++;
            // Vitesse
            //_moveSpeed = Random.Range(_moveSpeedMin, _moveSpeedMax);
            //ghostBehavior.SetSpeed(_moveSpeed);


        //    switch (SuperManager.instance.gameManagerAFSFOR.indexPalier)
        //    {
        //        case 0:
        //            // Life
        //            ghostBehavior.Set_Life(lifesPalier[0]);
        //            spawnIntervalActual = _spawnInterval[0];
        //            ghostBehavior.Set_BaseMat(baseMats[0]);
        //            break;

        //        case 1:
        //            // Life
        //            ghostBehavior.Set_Life(lifesPalier[1]);
        //            spawnIntervalActual = _spawnInterval[1];
        //            ghostBehavior.Set_BaseMat(baseMats[1]);
        //            break;

        //        case 2:
        //            // Life
        //            ghostBehavior.Set_Life(lifesPalier[2]);
        //            spawnIntervalActual = _spawnInterval[2];
        //            ghostBehavior.Set_BaseMat(baseMats[2]);
        //            break;
        //    }

        //    allGhosts.Add(ghostBehavior.transform);
        }

        yield return new WaitForSeconds(spawnIntervalActual);
        StartCoroutine(SpawnPrefabs());
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
    public bool Get_CanSpawn()
    {
        return _canSpawn;
    }

    public void RemoveGhostFromListAndDestroy(int idGhost)
    {
        allGhosts.RemoveAll(t =>
        {
            Ghost g = t.gameObject.GetComponent<Ghost>();
            if (g.GetId() == idGhost)
            {
                Destroy(t.gameObject);
                return true;
            }
            return false;
        });
    }

    public void StopWaveAndClearAllGhost()
    {
        SetCanSpawn(false);

        foreach (Transform t in allGhosts)
        {
            t.gameObject.GetComponent<Ghost>().KillAndDestroyGhost(true);
        }

        PlayOnceSoundKillGhost();
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
            t.gameObject.GetComponent<Ghost>().KillAndDestroyGhost(true);
        }
        PlayOnceSoundKillGhost();
        allGhosts.Clear();
    }

    private Transform GetRandomTargetBetweenMainAndLow()
    {
        int randomIndex = _currentIdGhost % 5;

        if (randomIndex == 0)
        {
            return _lowTarget;
        }
        else
        {
            return _mainTarget;
        }
    }
    public void UpdateMaxGhost(int indexPalier)
    {
        switch (indexPalier)
        {
            case 0:
                _maxGhostInTotal = SuperManager.instance.gameManagerAFSFOR.palierKills[0];
                break;

            case 1:
                _maxGhostInTotal = SuperManager.instance.gameManagerAFSFOR.palierKills[1];
                break;

            case 2:
                _maxGhostInTotal = SuperManager.instance.gameManagerAFSFOR.palierKills[2];
                break;
        }
    }

    private void PlayOnceSoundKillGhost()
    {
        //SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.BeehGoatReverb, 0.5f,new Vector3(0,0,0));
    }
}

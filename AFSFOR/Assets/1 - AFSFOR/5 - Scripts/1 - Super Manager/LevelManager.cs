using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public enum LevelType
{
    HUB,
    Tutorial,
    Level01,
    HubWithoutPlayer
}

public class LevelManager : MonoBehaviour
{
    [Header("Current Level in Game")]
    [SerializeField] private LevelType _currentLevel = LevelType.HUB;

    [Header("Names of all Game Levels")]
    [SerializeField] private string _nameLevel_HUB = "Scene - AFSFOR - HUB";
    [SerializeField] private string _nameLevel_Tutorial = "Scene - AFSFOR - Tutorial";
    [SerializeField] private string _nameLevel_Level01 = "Scene - AFSFOR - Level 1";
    [SerializeField] private string _nameLevel_HUBwithoutPlayer = "Scene - AFSFOR - HUBwithoutPlayer";
    [SerializeField] private string _currentNameLevel;
    [SerializeField] private string _oldNameLevel;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        _currentNameLevel = _nameLevel_HUB;

        if (_currentLevel != LevelType.HUB)
        {
            LoadLevel(_currentLevel);
        }
    }

    public string Get_currentNameLevel()
    {
        return _currentNameLevel;
    }
    public LevelType GetCurrentLevelType()
    {
        return _currentLevel;
    }

    public void LoadLevel(LevelType levelType)
    {
        _oldNameLevel = _currentNameLevel;

        string levelName = "";

        switch (levelType)
        {
            case LevelType.HUB:
                //levelName = _nameLevel_HUB;
                levelName = _nameLevel_HUBwithoutPlayer;
                break;

            case LevelType.Tutorial:
                levelName = _nameLevel_Tutorial;
                break;

            case LevelType.Level01:
                levelName = _nameLevel_Level01;
                break;

        }

        _currentNameLevel = levelName; 
        _currentLevel = levelType;
        StartCoroutine(LoadingNextSceneFading());
    }
    IEnumerator LoadingNextSceneFading()
    {
        yield return new WaitForSeconds(2.0f);
        Player.instance.gameObject.GetComponent<Player_AFSFOR>().RefillPlayerLife();
        SuperManager.instance.soundManager.DestroyAllSoundsPrefabs3D();
        SceneManager.LoadSceneAsync(_currentNameLevel);
        yield return new WaitForSeconds(0.5f);
        //SuperManager.instance.uiManager.GetPostProcessVolumeInScene(_currentLevel);
        //SuperManager.instance.vibrationManager.InitializeVibrationManager();
    }

    //  ###########################################

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadLevel(LevelType.HUB);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {

            LoadLevel(LevelType.Tutorial);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadLevel(LevelType.Level01);

        }
    }

    public void BackToHub()
    {
        SuperManager.instance.uiManager.VisibleToBlack();
        LoadLevel(LevelType.HUB);
    }

    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        //SuperManager.instance.gameManagerAFSFOR.LaunchGameplay(_currentLevel);
    }

    public GameObject FindInScene(LevelType levelType, string gameObjectName)
    {
        string sceneName = "";

        switch (levelType)
        {
            case LevelType.Tutorial:
                sceneName = _nameLevel_Tutorial;
                break;
            case LevelType.Level01:
                sceneName = _nameLevel_Level01;
                break;
            case LevelType.HUB:
                sceneName = _nameLevel_HUB;
                break;
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    if (obj.name == gameObjectName)
                    {
                        return obj;
                    }
                }
            }
        }

        Debug.LogWarning($"GameObject '{gameObjectName}' not found in scene '{sceneName}'.");
        return null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

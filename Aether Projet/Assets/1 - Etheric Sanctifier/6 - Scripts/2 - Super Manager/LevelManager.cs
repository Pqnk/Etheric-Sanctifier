using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelType
{
    HUB,
    Tutorial,
    Level01
}


public class LevelManager : MonoBehaviour
{
    [Header("Current Level in Game")]
    [SerializeField] private LevelType _currentLevel = LevelType.HUB;

    [Header("Names of all Game Levels")]
    [SerializeField] private string _nameLevel_HUB = "Scene - Etheric Sanctifier - HUB";
    [SerializeField] private string _nameLevel_Tutorial = "Scene - Etheric Sanctifier - Tutorial";
    [SerializeField] private string _nameLevel_Level01 = "Scene - Etheric Sanctifier - Level 1";

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        //  Don't freak out, Robin. It will be useful for the tests !
        if (_currentLevel != LevelType.HUB)
        {
            LoadLevel(_currentLevel);
        }
    }

    public void LoadLevel(LevelType levelType)
    {
        string levelName = "";

        switch (levelType)
        {
            case LevelType.HUB:
                levelName = _nameLevel_HUB;
                break;

            case LevelType.Tutorial:
                levelName = _nameLevel_Tutorial;
                break;

            case LevelType.Level01:
                levelName = _nameLevel_Level01;
                break;

        }

        Application.LoadLevel(levelName);
        _currentLevel = levelType;
    }

    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        SuperManager.instance.gameManagerAetherPunk.LaunchGameplay(_currentLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
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

            Debug.Log(scene.name);

            if (scene.name == sceneName)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    if (obj.name == gameObjectName)
                    {
                        return obj;
                    }

                    Debug.Log(obj.name);
                }
            }
        }

        Debug.LogWarning($"GameObject '{gameObjectName}' not found in scene '{sceneName}'.");
        return null;
    }
}
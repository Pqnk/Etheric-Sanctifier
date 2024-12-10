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
    [SerializeField] private string _currentNameLevel;
    [SerializeField] private string _oldNameLevel;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        StartCoroutine(SuperManager.instance.uiManager.FadeToTransparent());

        _currentNameLevel = _nameLevel_HUB;

        if (_currentLevel != LevelType.HUB)
        {
            LoadLevel(_currentLevel);
        }
    }

    public void LoadLevel(LevelType levelType)
    {
        _oldNameLevel = _currentNameLevel;

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

        //SceneManager.LoadScene(levelName);

        //SceneManager.LoadSceneAsync(levelName);
        //LoadSceneAsync(levelName);

        _currentNameLevel = levelName; 
        _currentLevel = levelType;


        StartCoroutine(SuperManager.instance.uiManager.FadeToOpaque());
        StartCoroutine(LoadingNextSceneFading());
    }


    IEnumerator LoadingNextSceneFading()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadSceneAsync(_currentNameLevel);
        StartCoroutine(SuperManager.instance.uiManager.FadeToTransparent());
    }


    //  ###########################################
    //  Async Loading of Scenes
    //  ###########################################
    private AsyncOperation _asyncOperation;
    private bool _isSceneLoaded = false;
    public void LoadSceneInBackground(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        StartCoroutine(SuperManager.instance.uiManager.FadeToOpaque());

        float alarm = Time.time + 10.0f;
        while (alarm > Time.time && !IsSceneReady())
        {
            Debug.Log("Coucou ! ");
        }

        SwitchToLoadedScene();
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _asyncOperation.allowSceneActivation = false;

        while (!_asyncOperation.isDone)
        {
            Debug.Log($"Loading scene : {_asyncOperation.progress * 100}%");

            if (_asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Scene ready to be activated !");
                _isSceneLoaded = true;
                break;
            }

            yield return null;
        }
    }
    public bool IsSceneReady()
    {
        return _isSceneLoaded;
    }
    public void SwitchToLoadedScene()
    {
        if (_isSceneLoaded && _asyncOperation != null)
        {
            _asyncOperation.allowSceneActivation = true;
            StartCoroutine(SuperManager.instance.uiManager.FadeToTransparent());
            _isSceneLoaded = false;
            UnloadOldScene();
        }
        else
        {
            Debug.LogWarning("La scène n'est pas encore prête !");
        }
    }
    public void UnloadOldScene()
    {
        SceneManager.UnloadSceneAsync(_oldNameLevel);
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

    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        SuperManager.instance.gameManagerAetherPunk.LaunchGameplay(_currentLevel);
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



    public void QuitGame()
    {
        Application.Quit();
    }
}

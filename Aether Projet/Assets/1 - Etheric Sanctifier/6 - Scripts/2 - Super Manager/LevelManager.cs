using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        SuperManager.instance.gameManagerAetherPunk.LaunchGameplay(_currentLevel);

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}

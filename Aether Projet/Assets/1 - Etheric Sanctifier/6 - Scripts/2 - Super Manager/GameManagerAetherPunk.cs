using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManagerAetherPunk : MonoBehaviour
{
    public void LaunchGameplay(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.HUB:
                GameplayHUB();
                break;

            case LevelType.Tutorial:
                GameplayTutorial();
                break;

            case LevelType.Level01:
                GameplayLevel01();
                break;
        }
    }


    //  ####################################################
    //  HUB
    //  ####################################################
    private void GameplayHUB()
    {

    }


    //  ####################################################
    //  Tutorial
    //  ####################################################
    private void GameplayTutorial()
    {
        if (SuperManager.instance.ghostManager.InitializeghostManager(true))
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, SuperManager.instance.ghostManager.timeBetweenWave, true));
        }
        else
        {
            Debug.Log("ERROR : Ghost Manager cannot initialize into tutorial!");
        }
    }


    //  ####################################################
    //  Level 01
    //  ####################################################
    private int currentKill;
    public List<int> palierKills = new List<int>() { 10, 20, 30 };
    private int indexPalier = 0;

    public void Set_KillGhost(bool isReset)
    {
        if (isReset)
        {
            currentKill = 0;
        }
        else
        {
            currentKill++;
        }
    }
    public int Get_KillGhost()
    {
        return currentKill;
    }
    public void Set_NextPalier(bool isValidate)
    {
        if (isValidate)
        {
            indexPalier++;
        }
        else
        {
            indexPalier--;
        }
    }
    public int Get_Palier()
    {
        return indexPalier;
    }

    private void GameplayLevel01()
    {
        if (SuperManager.instance.ghostManager.InitializeghostManager(false))
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, 15.0f, false));
        }
        else
        {
            Debug.Log("ERROR : Ghost Manager cannot initialize into level 01!");
        }
    }
    IEnumerator ToggleGhostWaveWithDelay(bool toggle, float delay, bool istuto)
    {
        yield return new WaitForSeconds(delay);
        ToggleGhostWave(toggle, istuto);
    }
    public void ToggleGhostWave(bool toggle, bool isTuto)
    {
        if (toggle)
        {
            SuperManager.instance.ghostManager.SetCanSpawn(true, isTuto);
        }
        else
        {
            SuperManager.instance.ghostManager.SetCanSpawn(false, isTuto);
        }
    }


    //  ####################################################
    //  Player Managment
    //  ####################################################
    public void PlayerDeadCinematic()
    {
        StartCoroutine(DeathCinematic());
    }
    IEnumerator DeathCinematic()
    {
        yield return new WaitForSeconds(3.0f);

        SuperManager.instance.levelManager.LoadLevel(LevelType.HUB);
    }
}

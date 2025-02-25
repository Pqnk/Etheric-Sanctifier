using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;

public class GameManagerAFSFOR : MonoBehaviour
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
    //  HUBMusic
    //  ####################################################
    private void GameplayHUB()
    {
        SuperManager.instance.soundManager.PlaySound(SoundType.HUBMusic, 0.1f);
        SuperManager.instance.ghostManager.DefinitiveStopWaveAndClearGhosts();
    }

    //  ####################################################
    //  Tutorial
    //  ####################################################
    private void GameplayTutorial()
    {
        SuperManager.instance.soundManager.PlaySound(SoundType.MusicTuto, 0.1f);

        if (SuperManager.instance.ghostManager.InitializeghostManager(true))
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, 10.0f));
        }
        else
        {
            Debug.Log("ERROR : Ghost Manager cannot initialize into tutorial!");
        }
    }

    //  ####################################################
    //  Level 01
    //  ####################################################
    public int currentKill;
    public int indexPalier = 0;
    public List<int> palierKills = new List<int>() { 10, 20, 30 };

    private void GameplayLevel01()
    {
        SuperManager.instance.soundManager.PlaySound(SoundType.Music, 0.1f);

        if (SuperManager.instance.ghostManager.InitializeghostManager(false))
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, 15.0f));
        }
        else
        {
            Debug.Log("ERROR : Ghost Manager cannot initialize into level 01!");
        }
    }

    public void Set_KillGhost(bool minus)
    {
        if (minus)
        {
            currentKill--;
        }
        else
        {
            currentKill++;
        }
    }
    public void Set_ResetGhost()
    {
        currentKill = 0;
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

        SuperManager.instance.ghostManager.StopWaveAndClearAllGhost();
        SuperManager.instance.ghostManager.UpdateMaxGhost(indexPalier);
    }
    public int Get_Palier()
    {
        return indexPalier;
    }

    IEnumerator ToggleGhostWaveWithDelay(bool toggle, float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleGhostWave(toggle);
    }
    public void ToggleGhostWave(bool toggle)
    {
        if (toggle)
        {
            SuperManager.instance.ghostManager.SetCanSpawn(true);
        }
        else
        {
            SuperManager.instance.ghostManager.SetCanSpawn(false);
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

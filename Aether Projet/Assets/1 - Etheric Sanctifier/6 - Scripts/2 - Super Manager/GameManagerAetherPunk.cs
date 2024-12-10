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
        SuperManager.instance.ghostManager.DefinitiveStopWaveAndClearGhosts();
        SuperManager.instance.soundManager.PlaySound(SoundType.HUB, 0.1f);
    }


    //  ####################################################
    //  Tutorial
    //  ####################################################
    private void GameplayTutorial()
    {
        SuperManager.instance.soundManager.PlaySound(SoundType.MusicTuto, 0.1f);

        if (SuperManager.instance.ghostManager.InitializeghostManager(true))
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, 10.0f, true));
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
            SuperManager.instance.voiceManager.PlayVoiceAtLocation(VoiceType.BriefingMission, 0.5f, SuperManager.instance.ghostManager.GetMainTarget());
            StartCoroutine(ToggleGhostWaveWithDelay(true, 15.0f, false));
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
            SuperManager.instance.ghostManager.StopWaveAndClearAllGhost();
        }
        else
        {
            indexPalier--;
            SuperManager.instance.ghostManager.StopWaveAndClearAllGhost();
        }
    }
    public int Get_Palier()
    {
        return indexPalier;
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

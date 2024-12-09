using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManagerAetherPunk : MonoBehaviour
{
    private void Awake()
    {

    }

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

    private void GameplayHUB()
    {

    }

    private void GameplayTutorial()
    {

    }

    private void GameplayLevel01()
    {
        if (SuperManager.instance.ghostManager.InitializeghostManager())
        {
            StartCoroutine(ToggleGhostWaveWithDelay(true, 15.0f));
        }
        else
        {
            Debug.Log("ERROR : Ghost Manager cannot initialize ! Go check JaaJ, he must know what is happening !");
        }
    }

    IEnumerator ToggleGhostWaveWithDelay(bool toggle, float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleGhostWave(toggle);
    }

    private void ToggleGhostWave(bool toggle)
    {
        if(toggle)
        {
            SuperManager.instance.ghostManager.SetCanSpawn(true);
        }
        else
        {
            SuperManager.instance.ghostManager.SetCanSpawn(false);
        }
    }

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

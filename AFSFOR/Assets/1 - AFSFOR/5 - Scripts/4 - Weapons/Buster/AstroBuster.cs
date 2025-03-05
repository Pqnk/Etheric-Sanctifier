using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBuster : MonoBehaviour
{
    [Header("Parametre Light")]
    [SerializeField] Light busterLight;
    [SerializeField] Transform busterParticule;
    [SerializeField] float currentRangeLight;

    float[] valeurLightPalier = { 17, 20, 30, 30 };
    float[] valeurParticulePalier = { .2f, .5f, 1, 1.7f };

    bool isBossWave = false;

    private void Start()
    {
        ResetAstroBuster();
    }

    private void Update()
    {
        busterLight.range = currentRangeLight;
    }

    public void Add_Point()
    {
        GameManager.instance.enemiesKilledThisWave++;

        int currentWave = GameManager.instance.currentWaveIndexGlobal;
        int enemiesKilled = GameManager.instance.enemiesKilledThisWave;
        int enemiesToKill = GameManager.instance.enemiesToKillThisWave;

        int waveStep = (currentWave - 1) % 3;
        isBossWave = (waveStep == 2);

        if (isBossWave)
        {
            BossAstroBuster();
        }
        else
        {
            UpdateBusterSize(waveStep, enemiesKilled, enemiesToKill);
            UpdateBusterLight(waveStep, enemiesKilled, enemiesToKill);
        }

        if (IsWaveFinished())
        {
            if (isBossWave)
            {
                StartCoroutine(WaitBeforeReset());
            }
        }
    }

    void ResetAstroBuster()
    {
        isBossWave = false;
        currentRangeLight = valeurLightPalier[0];
        busterParticule.localScale = Vector3.one * valeurParticulePalier[0];
    }
    void BossAstroBuster()
    {
        currentRangeLight = valeurLightPalier[3];
        busterParticule.localScale = Vector3.one * valeurParticulePalier[3];
    }

    void UpdateBusterSize(int waveStep, int enemiesKilled, int enemiesToKill)
    {
        if (enemiesToKill == 0 || isBossWave) return;

        float progress = Mathf.Clamp01((float)enemiesKilled / enemiesToKill);
        float targetScale = Mathf.Lerp(valeurParticulePalier[waveStep], valeurParticulePalier[waveStep + 1], progress);

        busterParticule.localScale = Vector3.one * targetScale;
    }

    void UpdateBusterLight(int waveStep, int enemiesKilled, int enemiesToKill)
    {
        if (enemiesToKill == 0 || isBossWave) return;

        float progress = Mathf.Clamp01((float)enemiesKilled / enemiesToKill);
        float targetLight = Mathf.Lerp(valeurLightPalier[waveStep], valeurLightPalier[waveStep + 1], progress);

        currentRangeLight = targetLight;
    }

    bool IsWaveFinished()
    {
        return GameManager.instance.enemiesKilledThisWave >= GameManager.instance.enemiesToKillThisWave;
    }    

    IEnumerator WaitBeforeReset()
    {
        yield return new WaitForSeconds(5f);
        ResetAstroBuster();
    }
}

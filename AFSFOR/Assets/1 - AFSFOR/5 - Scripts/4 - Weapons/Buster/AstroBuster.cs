using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBuster : MonoBehaviour
{
    [Header("Parametre Light")]
    [SerializeField] Light busterLight;
    [SerializeField] Transform busterParticule;
    [SerializeField] float currentRangeLight;

    float[] valeurLightPalier = { 10, 20, 30, 30 };
    float[] valeurParticulePalier = { .2f, .5f, 1, 1.7f };

    int currentPoints = 0;

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

        if (IsWaveFinished())
        {
            if ((GameManager.instance.currentWaveIndex + 1) % 3 == 0) 
            {
                StartCoroutine(WaitBeforeReset());
            }
        }
        else
        {
            UpdateBusterSize();
            UpdateBusterLight();
        }
    }

    void ResetAstroBuster()
    {
        currentRangeLight = valeurLightPalier[0];
        busterParticule.localScale = new Vector3(valeurParticulePalier[0], valeurParticulePalier[0], valeurParticulePalier[0]);
    }

    void UpdateBusterSize()
    {
        int currentWave = GameManager.instance.currentWaveIndex;
        int enemiesKilled = GameManager.instance.enemiesKilledThisWave;
        int enemiesToKill = GameManager.instance.enemiesToKillThisWave;

        if (enemiesToKill == 0 || currentWave >= valeurParticulePalier.Length - 1) return;

        float progress = Mathf.Clamp01((float)enemiesKilled / enemiesToKill);
        float scaleValue = Mathf.Lerp(valeurParticulePalier[currentWave], valeurParticulePalier[currentWave + 1], progress);

        busterParticule.localScale = Vector3.one * scaleValue;
    }

    void UpdateBusterLight()
    {
        int currentWave = GameManager.instance.currentWaveIndex;
        int enemiesKilled = GameManager.instance.enemiesKilledThisWave;
        int enemiesToKill = GameManager.instance.enemiesToKillThisWave;

        if (enemiesToKill == 0 || currentWave >= valeurLightPalier.Length - 1) return;

        float progress = Mathf.Clamp01((float)enemiesKilled / enemiesToKill);
        float lightValue = Mathf.Lerp(valeurLightPalier[currentWave], valeurLightPalier[currentWave + 1], progress);

        currentRangeLight = lightValue;
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

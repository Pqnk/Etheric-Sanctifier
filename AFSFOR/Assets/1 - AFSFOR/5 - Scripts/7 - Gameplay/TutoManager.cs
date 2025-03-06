using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum TutoButton
{
    Precedent,
    Suivant,
    Mana,
    Skip
}

public class TutoManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] spawnPoints;

    [Header("Tutoriel Info")]
    [SerializeField] GameObject tableTuto;
    [SerializeField] GameObject manaBtn;
    [SerializeField] GameObject suivantBtn;
    [SerializeField] GameObject precedentBtn;
    [SerializeField] GameObject contenerInfo;
    [SerializeField] int indexTuto = 0;
    [SerializeField] int totalInfoTuto = 0;

    Transform player;
    public bool playTuto = false;


    void Start()
    {
        totalInfoTuto = contenerInfo.transform.childCount;

        Player.instance.gameObject.GetComponent<Player_AFSFOR>().ToggleHeadLight(true);
        player = Player.instance.transform;

        ShowInfo(indexTuto);
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitUntil(() => playTuto);

        yield return new WaitForSeconds(1);

        while (true)
        {
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().targetProjectile = Player.instance.GetComponent<Player_AFSFOR>().VRHead.transform;
            yield return new WaitForSeconds(5);
        }
    }

    public void AffichageTutoButton(TutoButton etatTuto)
    {
        switch (etatTuto)
        {
            case TutoButton.Precedent:
                if (indexTuto > 0)
                {
                    indexTuto--;
                    ShowInfo(indexTuto);
                }
                break;

            case TutoButton.Suivant:
                if (indexTuto < totalInfoTuto - 1)
                {
                    indexTuto++;
                    ShowInfo(indexTuto);
                }
                break;

            case TutoButton.Mana:
                Player.instance.gameObject.GetComponent<Player_AFSFOR>().AddMana(20000);
                break;

            case TutoButton.Skip:
                tableTuto.SetActive(false);
                playTuto = true;
                break;
        }
    }

    void ShowInfo(int index)
    {
        if (index == 4)
        {
            manaBtn.SetActive(true);
        }
        else
        {
            manaBtn.SetActive(false);              
        }

        for (int i = 0; i < totalInfoTuto; i++)
        {
            contenerInfo.transform.GetChild(i).gameObject.SetActive(i == index);
        }

        // Mettre à jour les boutons
        precedentBtn.SetActive(index > 0);
        suivantBtn.SetActive(index < totalInfoTuto - 1);
    }
}

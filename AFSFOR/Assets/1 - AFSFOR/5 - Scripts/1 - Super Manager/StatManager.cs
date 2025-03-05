using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("PLAYER")]
    public int damagePlayerSWORD;
    public int damagePlayerGUNLIGHT;
    public int damagePlayerGUNHEAVY;
    public int damagePlayerHAND;
    public int lifePlayer;

    [Header("ENEMIES")]
    [Header("Walker")]
    public int damageWALKER;
    public int lifeWALKER;
    public float speedWALKER;

    [Header("Tireur")]
    public int damageTIREUR;
    public int lifeTIREUR;
    public float speedTIREUR;

    [Header("Flying")]
    public int damageFlying;
    public int lifeFlying;
    public float speedFlying;

    [Header("Boss")]
    public int damageBOSS;
    public int lifeBOSS;
    public float speedBOSS;


    [Header("UI")]
    public GameObject hitcanvas;


    public void SpawnHitCanvas(int hitDamage, Vector3 pos)
    {
        GameObject newHitUi = Instantiate(hitcanvas, pos + new Vector3(0,0.3f,0), Quaternion.identity);

        newHitUi.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = hitDamage.ToString();
    }
}

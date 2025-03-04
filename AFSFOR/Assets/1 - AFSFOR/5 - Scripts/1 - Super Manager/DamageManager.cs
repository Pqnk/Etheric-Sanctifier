using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("PLAYER")]
    public int damagePlayerSWORD;
    public int damagePlayerGUN;
    public int damagePlayerHAND;
    public int lifePlayer;

    [Header("ENEMIES")]
    [Header("Walker")]
    public int damageWALKER;
    public int lifeWALKER;

    [Header("Tireur")]
    public int damageTIREUR;
    public int lifeTIREUR;

    [Header("Ghost")]
    public int damageGHOST;
    public int lifeGHOST;

    [Header("Boss")]
    public int damageBOSS;
    public int lifeBOSS;


    [Header("UI")]
    public GameObject hitcanvas;


    public void SpawnHitCanvas(int hitDamage, Vector3 pos)
    {
        GameObject newHitUi = Instantiate(hitcanvas, pos + new Vector3(0,0.3f,0), Quaternion.identity);

        newHitUi.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = hitDamage.ToString();
    }
}

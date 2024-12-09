using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Pistol : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform grabPoint;


    [Header("Weapon")]
    [SerializeField] int damage;
    [SerializeField] float forcePush;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefabs;


    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Input_Sources handType;

    void Update()
    {
        // Vérifier si le trigger est pressé
        if (triggerAction.GetStateDown(handType))
        {
            Debug.Log("Trigger pressed on: " + handType);
            PerformAction();
        }
    }

    void PerformAction()
    {
        GameObject go = Instantiate(bulletPrefabs, shootPoint.position, shootPoint.rotation);
        go.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        go.GetComponent<Bullet>().forcePush = forcePush;
        go.GetComponent<Bullet>().damage = damage;
    }
}

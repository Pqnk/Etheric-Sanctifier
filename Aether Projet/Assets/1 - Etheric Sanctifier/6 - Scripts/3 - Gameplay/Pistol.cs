using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;
using Valve.VR;

public class Pistol : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform grabPoint;

    [Header("Grab")]
    [SerializeField] private float grabRange = 2f;
    [SerializeField] private LayerMask grabbableLayer;

    [Header("Weapon LightShoot")]
    [SerializeField] int lightDamage;
    [SerializeField] float forcePush;
    [SerializeField] float lightBulletSpeed;
    [SerializeField] GameObject lightBulletPrefabs;

    [Header("Weapon HeavyShoot")]
    [SerializeField] float heavyBulletSpeed;
    [SerializeField] GameObject heavyBulletPrefabs;
    [SerializeField] float timeForShooting;
    private float currentTimerShoot = 0;

    [Header("Input")]
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean triggerGrab;
    public SteamVR_Input_Sources handType;

    private Player player;

    private void Start()
    {
        player = transform.root.GetComponent<Player>();
    }

    void Update()
    {
        if (triggerAction.GetStateDown(handType))
        {
            //Debug.Log("Trigger pressed on: " + handType);
            Perform_Shoot();
        }

        if (triggerAction.GetState(handType))
        {
            if (player.Get_playerCurrentMana() >= player.Get_playerMaxMana())
            {
                currentTimerShoot += Time.deltaTime;

                if (currentTimerShoot >= timeForShooting)
                {
                    Debug.Log("Trigger Long pressed on: " + handType);
                    player.AsShootRail();
                    currentTimerShoot = 0;
                    Perform_ShootRail();
                }
            }
        }
    }


    void Perform_Shoot()
    {
        GameObject lightBullet = Instantiate(lightBulletPrefabs, shootPoint.position, shootPoint.rotation);
        lightBullet.GetComponent<Bullet>().bulletSpeed = lightBulletSpeed;
        lightBullet.GetComponent<Bullet>().forcePush = forcePush;
        lightBullet.GetComponent<Bullet>().damage = lightDamage;
    }

    void Perform_ShootRail()
    {
        GameObject heavyBullet = Instantiate(heavyBulletPrefabs, shootPoint.position, shootPoint.rotation);
        heavyBullet.GetComponent<Bullet>().bulletSpeed = heavyBulletSpeed;
        heavyBullet.GetComponent<Bullet>().isHeavyShoot = true;
        heavyBullet.GetComponent<Bullet>().damage = 10000;
    }

    private void OnDrawGizmosSelected()
    {
        if (grabPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(grabPoint.position, grabRange);
        }
    }
}

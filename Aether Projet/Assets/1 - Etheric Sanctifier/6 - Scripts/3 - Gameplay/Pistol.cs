using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
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
    [SerializeField] float timeSizeMaxShooting;
    [SerializeField] float upSizeMaxShooting;
    [SerializeField] float rangeHeavyImpact;
    private float currentTimerShoot = 0;

    [Header("Input")]
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean triggerGrab;
    public SteamVR_Input_Sources handType;

    [Header("Materials")]
    [SerializeField] List<Material> outOfMana_Mat;
    [SerializeField] List<Material> fullOfMana_Mat;

    private Player player;
    private bool readyHeavyShoot = false;
    private bool getGameObjectShoot = false;
    private bool isIntantiate = false;
    private bool chargingSound = true;
    private GameObject chargingShoot = null;
    private VisualEffect visual = null;

    private void Start()
    {
        player = transform.root.GetComponent<Player>();
    }
    void Update()
    {
        SimpleShootInput();
        HeavyShootInput();
        CheckManaReady();
    }


    //  ###########################################
    //  #############  PISTOL MANA  ###############
    //  ###########################################
    private void CheckManaReady()
    {
        if (player.Get_playerCurrentMana() >= player.Get_playerMaxMana())
        {
            if (chargingSound)
            {
                chargingSound = false;
                SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.BigShootReady, 0.5f, shootPoint.position);
            }
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().SetMaterials(fullOfMana_Mat);
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().SetMaterials(outOfMana_Mat);
        }
    }

    //  ###########################################
    //  ############  PISTOL INPUTS  ##############
    //  ###########################################
    private void SimpleShootInput()
    {
        if (triggerAction.GetStateDown(handType))
        {
            Perform_Shoot();
        }
    }
    private void HeavyShootInput()
    {
        if (triggerAction.GetState(handType))
        {
            Coroutine hapticCoroutine = null;

            if (triggerAction.GetStateDown(handType))
            {
                if (hapticCoroutine == null)
                {
                    hapticCoroutine = StartCoroutine(SuperManager.instance.vibrationManager.leftController.StartHapticFeedback()); ;
                }
            }
            else if (triggerAction.GetStateUp(handType))
            {
                if (hapticCoroutine != null)
                {
                    StopCoroutine(hapticCoroutine);
                    hapticCoroutine = null;
                }
            }


            if (player.Get_playerCurrentMana() >= player.Get_playerMaxMana())
            {
                if (!getGameObjectShoot)
                {
                    getGameObjectShoot = true;
                    chargingShoot = SuperManager.instance.vfxManager.InstantiateVFX_VFXChargingHeavyShoot(shootPoint);
                    chargingShoot.transform.parent = transform;
                    visual = chargingShoot.GetComponent<VisualEffect>();
                }

                currentTimerShoot += Time.deltaTime;
                visual.SetFloat("Size", currentTimerShoot / timeSizeMaxShooting);

                if (currentTimerShoot >= timeForShooting)
                {
                    Debug.Log("Trigger Long pressed on: " + handType);
                    visual.SetFloat("Size", 1.2f);
                    readyHeavyShoot = true;
                }
            }
        }
        else
        {
            currentTimerShoot = 0;

            if (readyHeavyShoot)
            {
                readyHeavyShoot = false;
                getGameObjectShoot = false;
                player.AsShootRail();
                Perform_ShootRail();
                Destroy(chargingShoot);
            }
        }
    }

    //  ###########################################
    //  ############  PISTOL PERFORMS  ############
    //  ###########################################
    void Perform_Shoot()
    {
        SuperManager.instance.vibrationManager.leftController.ShootHaptic();

        PlaySoundShoot();
        GameObject lightBullet = Instantiate(lightBulletPrefabs, shootPoint.position, shootPoint.rotation);
        lightBullet.GetComponent<Bullet>().bulletSpeed = lightBulletSpeed;
        lightBullet.GetComponent<Bullet>().forcePush = forcePush;
        lightBullet.GetComponent<Bullet>().damage = lightDamage;
    }
    void Perform_ShootRail()
    {
        SuperManager.instance.vibrationManager.leftController.BigShootHaptic();
        PlaySoundBigShoot();

        GameObject heavyBullet = Instantiate(heavyBulletPrefabs, shootPoint.position, shootPoint.rotation);
        heavyBullet.GetComponent<Bullet>().bulletSpeed = heavyBulletSpeed;
        heavyBullet.GetComponent<Bullet>().isHeavyShoot = true;
        heavyBullet.GetComponent<Bullet>().rangeHeavyImpact = rangeHeavyImpact;
        heavyBullet.GetComponent<Bullet>().damage = 10000;
    }

    //  ###########################################
    //  ############  PISTOL SOUNDS  ##############
    //  ###########################################
    private void PlaySoundShoot()
    {
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Shoot, 0.5f, shootPoint.position);
    }
    private void PlaySoundBigShoot()
    {
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.ShootBig, 0.5f, shootPoint.position);
    }







    //  ###########################################
    //  ##############  DEBUGGING  ################
    //  ###########################################
    private void DebugChargingShoot()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (player.Get_playerCurrentMana() >= player.Get_playerMaxMana())
            {
                if (!getGameObjectShoot)
                {
                    getGameObjectShoot = true;
                    chargingShoot = SuperManager.instance.vfxManager.InstantiateVFX_VFXChargingHeavyShoot(shootPoint);
                    chargingShoot.transform.parent = transform;
                    visual = chargingShoot.GetComponent<VisualEffect>();
                }

                currentTimerShoot += Time.deltaTime;

                visual.SetFloat("Size", currentTimerShoot / timeSizeMaxShooting);

                if (currentTimerShoot >= timeForShooting)
                {
                    Debug.Log("Trigger Long pressed on: " + handType);
                    visual.SetFloat("Size", upSizeMaxShooting);
                    readyHeavyShoot = true;
                }
            }
        }
        else
        {
            currentTimerShoot = 0;

            if (readyHeavyShoot)
            {
                readyHeavyShoot = false;
                getGameObjectShoot = false;
                player.AsShootRail();
                Perform_ShootRail();
                Destroy(chargingShoot);
            }
        }
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

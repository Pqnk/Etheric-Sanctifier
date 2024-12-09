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

    [Header("Weapon")]
    [SerializeField] int damage;
    [SerializeField] float forcePush;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefabs;

    private Rigidbody grabbedObject;
    private Vector3 handVelocity;
    private Vector3 handAngularVelocity;

    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean triggerGrab;
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose pose;

    void Update()
    {
        if (pose != null)
        {
            handVelocity = pose.GetVelocity();
            handAngularVelocity = pose.GetAngularVelocity();
        }

        if (triggerAction.GetStateDown(handType))
        {
            Debug.Log("Trigger pressed on: " + handType);
            Perform_Shoot();
        }

        if (triggerGrab.GetState(handType))
        {
            if (grabbedObject == null)
            {
                TryGrab();
            }
        }
        else
        {
            ReleaseObject();
        }

        if (grabbedObject != null)
        {
            grabbedObject.MovePosition(grabPoint.position);
        }
    }

    private void TryGrab()
    {
        Collider[] hitColliders = Physics.OverlapSphere(grabPoint.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0)
        {
            Collider closestObject = hitColliders[0];
            float closestDistance = Vector3.Distance(grabPoint.position, closestObject.transform.position);

            foreach (Collider hit in hitColliders)
            {
                float distance = Vector3.Distance(grabPoint.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestObject = hit;
                    closestDistance = distance;
                }
            }

            grabbedObject = closestObject.GetComponent<Rigidbody>();
            if (grabbedObject != null)
            {
                grabbedObject.isKinematic = true;
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.isKinematic = false;

            Vector3 releaseForce = handVelocity * 10.5f + transform.forward * 5.0f;
            grabbedObject.velocity = handVelocity;
            grabbedObject.angularVelocity = handAngularVelocity;

            Grenade grenade = grabbedObject.GetComponent<Grenade>();
            if (grenade != null)
            {
                float velocityThreshold = 1.0f;
                grenade.estLancer = handVelocity.magnitude > velocityThreshold;
            }

            grabbedObject = null;
        }
    }

    void Perform_Shoot()
    {
        GameObject go = Instantiate(bulletPrefabs, shootPoint.position, shootPoint.rotation);
        go.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        go.GetComponent<Bullet>().forcePush = forcePush;
        go.GetComponent<Bullet>().damage = damage;
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

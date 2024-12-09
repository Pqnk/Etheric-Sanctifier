using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Settings")]
    public bool estLancer;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask detectionLayer;
   

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!estLancer || hasExploded) return;

        hasExploded = true;

        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Ghost"))
            {
                HandleGhostInteraction(collider.gameObject);
            }
        }

        Destroy(gameObject);
    }

    private void HandleGhostInteraction(GameObject ghost)
    {
        Debug.Log("Ghost trouvé : " + ghost.name);

        Destroy(ghost);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

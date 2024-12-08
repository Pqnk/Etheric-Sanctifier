using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionGhost : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 100f;
    private int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("GhostLayer");
    }

    void Update()
    {
        Vector3 direction = transform.forward;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, raycastDistance, layerMask))
        {
            if (hit.collider.CompareTag("Ghost"))
            {
                Ghost behaviorGhost = hit.collider.gameObject.GetComponent<Ghost>();
                behaviorGhost.SetIsDetected(true);
            }
        }

        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);
    }

}

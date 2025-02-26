using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DetectionUI : MonoBehaviour
{
    public float rayDistance = 10f;

    private LineRenderer lineRenderer;
    private int uiLayer;

    void Start()
    {
        // Récupération du LineRenderer et configuration
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        uiLayer = LayerMask.NameToLayer("UI");
    }

    void Update()
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = startPoint + transform.forward * rayDistance;
        bool uiDetected = false;

        if (Physics.Raycast(startPoint, transform.forward, out RaycastHit hit, rayDistance))
        {
            endPoint = hit.point;

            if (hit.collider.gameObject.layer == uiLayer)
            {
                uiDetected = true;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Bouton UI détecté!");
                }
            }
        }

        lineRenderer.enabled = uiDetected;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRUIRaycaster : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean clickAction;

    private LineRenderer lineRenderer;
    private RaycastHit hit;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 5f);

        if (Physics.Raycast(ray, out hit, 5f))
        {
            lineRenderer.SetPosition(1, hit.point);
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<UnityEngine.UI.Button>() && clickAction.GetStateDown(handType))
            {
                ExecuteEvents.Execute(hitObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                Debug.Log("Click!");
            }
        }
    }
}
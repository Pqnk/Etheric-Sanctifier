using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

public class VRUICollider : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean clickAction;

    private GameObject currentUIButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIButton"))
        {
            currentUIButton = other.gameObject;
            Debug.Log("Bouton détecté : " + currentUIButton.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentUIButton)
        {
            currentUIButton = null;
        }
    }

    private void Update()
    {
        if (currentUIButton != null && clickAction.GetStateDown(handType))
        {
            ExecuteEvents.Execute(currentUIButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            currentUIButton.GetComponent<Button>().onClick.Invoke();
            Debug.Log("Bouton cliqué !");
        }
    }
}

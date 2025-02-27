using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;
    public bool isPortalActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            DeactivatePortal();
            LoadLevelFromPortal();

            Debug.Log("Ok portail");
        }
    }
    private void LoadLevelFromPortal()
    {
        //SuperManager.instance.uiManager.VisibleToBlack();
        SuperManager.instance.levelManager.LoadLevel(_levelDestination);
    }
    public void DeactivatePortal()
    {
        isPortalActive = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Teleporting, 0.8f, this.transform.position);
        //SuperManager.instance.vibrationManager.leftController.TeleportHaptic();
        //SuperManager.instance.vibrationManager.rightController.TeleportHaptic();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;
    public bool isPortalActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            DeactivatePortal();
            LoadLevelFromPortal();
        }
    }
    private void LoadLevelFromPortal()
    {
        SuperManager.instance.uiManager.VisibleToBlack();
        SuperManager.instance.levelManager.LoadLevel(_levelDestination);
    }
    public void DeactivatePortal()
    {
        isPortalActive = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Teleporting, 0.5f, this.transform.position);
    }
}

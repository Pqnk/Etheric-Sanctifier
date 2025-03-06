using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LevelType _levelDestination = LevelType.HUB;
    public bool isPortalActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandNaked"))
        {
            StartCoroutine(DeactivatePortal(other.transform.position));
        }
    }
    IEnumerator DeactivatePortal(Vector3 position)
    {
        isPortalActive = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Teleporting, 0.8f, this.transform.position);
        SuperManager.instance.vfxManager.InstantiateVFX_vfxSwordImpact(position);
        SuperManager.instance.vibrationManager.leftController.TeleportHaptic();
        SuperManager.instance.vibrationManager.rightController.TeleportHaptic();
        yield return new WaitForSeconds(3.0f);
        LoadLevelFromPortal();
    }
    private void LoadLevelFromPortal()
    {
        SuperManager.instance.levelManager.LoadLevel(_levelDestination);
    }
}

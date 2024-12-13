using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFXType
{
    Explosion,
    GrenadeExplosion,
    ArcaneExplosion,
    DestructionGhost
}


public class VFXManager : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private GameObject _vfxHeavyImpact;
    [SerializeField] private GameObject _vfxFootImpact;
    [SerializeField] private GameObject _vfxLightImpact;
    [SerializeField] private GameObject _VFXChargingHeavyShoot;
    [SerializeField] private GameObject _VFXPalierValidated;
    [SerializeField] private GameObject _vfxDeadGhostImpact;
    [SerializeField] private GameObject _vfxSwordImpact;

    public GameObject InstantiateVFX_VFXChargingHeavyShoot(Transform pos)
    {
        GameObject instance = Instantiate(_VFXChargingHeavyShoot, pos.position, Quaternion.identity);

        return instance;
    }

    public GameObject InstantiateVFX_vfxHeavyImpact(Transform pos)
    {
        GameObject instance = Instantiate(_vfxHeavyImpact, pos.position, Quaternion.identity);

        instance.transform.SetParent(null);

        return instance;
    }

    public GameObject InstantiateVFX_VFXPalierValidated(Transform pos)
    {
        GameObject instance = Instantiate(_VFXPalierValidated, pos.position, Quaternion.identity);

        instance.transform.SetParent(null);

        return instance;
    }

    public GameObject InstantiateVFX_vfxFootImpact(Transform pos)
    {
        return Instantiate(_vfxFootImpact, pos.position, Quaternion.identity);
    }

    public GameObject InstantiateVFX_vfxLightImpact(Transform pos)
    {
        GameObject instance = Instantiate(_vfxLightImpact, pos.position, Quaternion.identity);

        instance.transform.SetParent(null);

        return instance;
    }

    public GameObject InstantiateVFX_vfxDeadGhostImpact(Transform pos)
    {
        GameObject instance = Instantiate(_vfxDeadGhostImpact, pos.position, Quaternion.identity);

        instance.transform.SetParent(null);

        return instance;
    }
    
    public GameObject InstantiateVFX_vfxSwordImpact(Transform pos)
    {
        GameObject instance = Instantiate(_vfxSwordImpact, pos.position, Quaternion.identity);

        instance.transform.SetParent(null);

        return instance;
    }

}

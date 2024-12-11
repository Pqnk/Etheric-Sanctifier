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
    [Header("Prefab VFX")]
    [SerializeField] private GameObject _VFXPrefab;

    [Header("VFX")]
    [SerializeField] private GameObject _vfxHeavyImpact;
    [SerializeField] private GameObject _vfxFootImpact;
    [SerializeField] private GameObject _vfxLightImpact;

    private GameObject InstantiateVFX()
    {
        return Instantiate(_VFXPrefab);
    }

    public GameObject InstantiateVFX_vfxHeavyImpact(Transform pos)
    {
        GameObject instance = Instantiate(_vfxHeavyImpact, pos.position, Quaternion.identity);

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

}

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
    [SerializeField] private GameObject _vfxExplosion;

    private GameObject InstantiateVFX()
    {
        return Instantiate(_VFXPrefab);
    }

}

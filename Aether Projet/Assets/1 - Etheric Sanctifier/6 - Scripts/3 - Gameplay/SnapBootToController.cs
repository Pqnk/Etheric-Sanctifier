using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SnapBootToController : MonoBehaviour
{
    public GameObject boot;

    void Start()
    {
        if (boot != null)
        {
            boot.transform.SetParent(this.transform);
            boot.transform.localPosition = Vector3.zero;
            boot.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("No boot found ! Make sure you've put a boot in the public field.");
        }
    }

}

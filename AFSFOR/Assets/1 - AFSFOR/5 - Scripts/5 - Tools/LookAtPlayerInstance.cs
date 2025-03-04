using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static UnityEngine.GraphicsBuffer;

public class LookAtPlayerInstance : MonoBehaviour
{
    private Transform _target;

    private void Start()
    {
        _target = Player.instance.transform;
    }

    void Update()
    {
        if (_target == null) return;
        transform.LookAt(_target);
    }
}

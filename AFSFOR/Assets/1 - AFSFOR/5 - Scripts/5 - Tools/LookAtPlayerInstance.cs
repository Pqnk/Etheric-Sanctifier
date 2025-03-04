using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static UnityEngine.GraphicsBuffer;

public class LookAtPlayerInstance : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Player.instance.gameObject.GetComponent<Player_AFSFOR>().VRHead.transform);
        transform.Translate(0, 0.01f, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Pistol : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform grabPoint;


    [Header("Weapon")]
    [SerializeField] int damage;
    [SerializeField] float forcePush;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

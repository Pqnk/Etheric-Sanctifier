using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public bool isLearned = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("J'ai une collision ! : " + other.gameObject.tag);

        if(other.gameObject.tag == "Sword")
        {
            isLearned = true;
        }
    }
}

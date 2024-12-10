using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yanis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SuperManager.instance.ghostManager.InitializeghostManager();

        SuperManager.instance.ghostManager.SetCanSpawn(true, false);
        SuperManager.instance.gameManagerAetherPunk.ToggleGhostWave(true, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yanis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SuperManager.instance.ghostManager.InitializeghostManager(false);

        SuperManager.instance.ghostManager.SetCanSpawn(true);
        SuperManager.instance.gameManagerAetherPunk.ToggleGhostWave(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

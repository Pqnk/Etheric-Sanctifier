using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_CallManager : MonoBehaviour
{
    public GameObject managerPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject manager = Instantiate(managerPrefab);
        manager.name = "--MANAGER--";
        manager.AddComponent<PersistOnLoad>();
    }

}

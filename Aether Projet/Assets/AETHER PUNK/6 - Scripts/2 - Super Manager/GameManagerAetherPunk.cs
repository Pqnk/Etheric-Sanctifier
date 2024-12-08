using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManagerAetherPunk : MonoBehaviour
{
    public List<GameObject> detectionEffects = new List<GameObject>();

    [Header("Search Target")]
    public GameObject artefactParent;
    public Material artefactMat;
    public float distanceSearch = 10f;
    public float cooldownSearch = 1f;

    [Header("Search Target")]
    public GameObject appearArea;
    public GameObject particulFollow;

    private void Awake()
    {
        artefactParent = GameObject.Find("ArtefactParent");
    }


    public void CheckVictory()
    {
        bool victory = false;

        foreach (Transform item in artefactParent.transform) 
        {
            string nameMat = item.gameObject.GetComponent<MeshRenderer>().material.name;
            string[] thisName = nameMat.Split(" ");

            if (thisName[0] != artefactMat.name)
            {
                victory = false;
            }
            else
            {
                victory = true;
            }
        }
    }
}

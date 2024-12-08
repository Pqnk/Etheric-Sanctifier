using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutoType
{
    Tp,
    Collision,
    Detection,
    PasTouche,
}

public class TutoManager : MonoBehaviour
{
    public GameObject directionnalLight;
    public GameObject whiteAreaUnderPlayer;

    [Header("Livres")]
    public GameObject livresParent;
    public bool isTpLearned;
    public bool isCollisionLearned;
    public bool isDetectionLearned;

    [Header("Porte")]
    public GameObject porte;

    [Header("Enviro Light Blanc")]
    public GameObject envirLight;


    private void Start()
    {
        directionnalLight = GameObject.Find("Directional Light");
        livresParent = GameObject.Find("Livre_Global");
        envirLight = GameObject.Find("WhiteLight");
        porte = GameObject.Find("Porte");
        whiteAreaUnderPlayer = GameObject.Find("WhiteAreaUnderPlayer");

        //directionnalLight.SetActive(false);
        whiteAreaUnderPlayer.SetActive(false);
        envirLight.SetActive(true);

        foreach (Transform item in livresParent.transform)
        {
            item.gameObject.SetActive(false);
        }

        livresParent.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PlayTpDialogue()
    {
        isTpLearned = true;
        //livresParent.transform.GetChild(2).gameObject.SetActive(true);
        // Play dialogue
    }

    public void PlayCollisionDialogue()
    {
        isCollisionLearned = true;
        //livresParent.transform.GetChild(3).gameObject.SetActive(true);
        // Play dialogue
    }

    public void PlayDetectionDialogue()
    {
        isDetectionLearned = true;
        // Play dialogue
    }

    public void CheckFinishTuto()
    {
        if (isTpLearned && isCollisionLearned && isDetectionLearned)
        {
            porte.gameObject.GetComponent<Animator>().Play("Open");
        }
    }

    public void PlayDestructionVue()
    {
        envirLight.SetActive(false);
        GetComponent<s_MaterialReset>().ResetMat();
        directionnalLight.SetActive(true);
        //whiteAreaUnderPlayer.SetActive(true);
        livresParent.transform.GetChild(0).gameObject.SetActive(false);

        //livresParent.transform.GetChild(1).gameObject.SetActive(true);

        StartCoroutine(TimerToBook());
    }

    IEnumerator TimerToBook()
    {
        yield return new WaitForSeconds(1);

        foreach (Transform item in livresParent.transform)
        {
            if (item.gameObject.name != "Livre_Destruction")
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
